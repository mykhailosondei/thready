using System.Diagnostics.CodeAnalysis;
using System.Text;
using ApplicationBLL.Exceptions;
using ApplicationBLL.QueryRepositories;
using ApplicationBLL.Services.Abstract;
using ApplicationDAL.Context;
using ApplicationDAL.Entities;
using ApplicationCommon.DTOs.User;
using ApplicationCommon.Interfaces;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using group_project_thread.Validators;
using Microsoft.EntityFrameworkCore;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace ApplicationBLL.Services;

public class UserService : BaseService
{
    private readonly EmailValidatorService _emailValidatorService;
    private readonly UserQueryRepository _userQueryRepository;
    private readonly PostQueryRepository _postQueryRepository;
    private readonly IValidator<RegisterUserDTO> _registerUserDTOValidator;
    private readonly IValidator<UserDTO> _userDTOValidator;
    
    public UserService(ApplicationContext applicationContext, IMapper mapper, 
        EmailValidatorService emailValidatorService,
        IValidator<RegisterUserDTO> registerUserDtoValidator,
        IValidator<UserDTO> userDTOValidator, UserQueryRepository userQueryRepository, PostQueryRepository postQueryRepository) : base(applicationContext, mapper)
    {
        _emailValidatorService = emailValidatorService;
        _registerUserDTOValidator = registerUserDtoValidator;
        _userDTOValidator = userDTOValidator;
        _userQueryRepository = userQueryRepository;
        _postQueryRepository = postQueryRepository;
    }

    public UserService(UserQueryRepository userQueryRepository, PostQueryRepository postQueryRepository) : base(null, null)
    {
        _userQueryRepository = userQueryRepository;
        _postQueryRepository = postQueryRepository;
    }
    
    
    public async Task Follow(int userId, int currentUserId)
    {
        var userToFollowModel = await _userQueryRepository.GetUserById(userId);
        var userThatFollowsModel = await _userQueryRepository.GetUserById(currentUserId);
        if (userToFollowModel == null)
        {
            throw new UserNotFoundException("User with specified id does not exist");
        }

        if (userThatFollowsModel.FollowingIds.Contains(userToFollowModel.Id) || userToFollowModel.FollowersIds.Contains(userThatFollowsModel.Id))
        {
            throw new InvalidOperationException("You are already following this user");
        }

        if (IsSameUser(userId, currentUserId))
        {
            throw new InvalidOperationException("You can not follow yourself");
        }
        
        IFollowing userToFollowModelEntity = _mapper.Map<User>(userToFollowModel);
        IFollower userThatFollowsModelEntity = _mapper.Map<User>(userThatFollowsModel);

        userToFollowModelEntity.FollowersIds.Add(userThatFollowsModel.Id);
        userThatFollowsModelEntity.FollowingIds.Add(userToFollowModel.Id);

        await FollowSaveChanges(subject: userThatFollowsModelEntity, @object: userToFollowModelEntity);
    }
    
    public async Task Unfollow(int userId, int currentUserId)
    {
        var userToUnfollowModel = await _userQueryRepository.GetUserById(userId);
        
        var userThatUnfollowsModel = await _userQueryRepository.GetUserById(currentUserId);

        if (userToUnfollowModel == null)
        {
            throw new UserNotFoundException("User with specified id does not exist");
        }

        if (!userThatUnfollowsModel.FollowingIds.Contains(userToUnfollowModel.Id) ||
            !userToUnfollowModel.FollowersIds.Contains(userThatUnfollowsModel.Id))
        {
            throw new InvalidOperationException("You do not follow this user");
        }
        
        if (IsSameUser(userId, currentUserId))
        {
            throw new InvalidOperationException("You can not unfollow yourself");
        }

        IFollowing userToUnfollowModelEntity = _mapper.Map<User>(userToUnfollowModel);
        IFollower userThatUnfollowsModelEntity = _mapper.Map<User>(userThatUnfollowsModel);

        userThatUnfollowsModelEntity.FollowingIds.Remove(userToUnfollowModel.Id);
        userToUnfollowModelEntity.FollowersIds.Remove(userThatUnfollowsModel.Id);

        await FollowSaveChanges(subject: userThatUnfollowsModelEntity, @object: userToUnfollowModelEntity);
    }

    private async Task FollowSaveChanges(IFollower subject, IFollowing @object)
    {
        _applicationContext.Attach(subject);
        _applicationContext.Attach(@object);

        _applicationContext.Entry(subject).Property(s => s.FollowingIds).IsModified = true;
        _applicationContext.Entry(@object).Property(o => o.FollowersIds).IsModified = true;

        await _applicationContext.SaveChangesAsync();
    }

    public async Task<UserDTO> CreateUser(RegisterUserDTO registerUserDto)
    {
        ValidationResult validationResult = await _registerUserDTOValidator.ValidateAsync(registerUserDto);
        
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors[0].ErrorMessage);
        }
        
        var userEntity = _mapper.Map<User>(registerUserDto);

        if (!await _emailValidatorService.IsEmailAvailable(registerUserDto.Email))
        {
            throw new UserAlreadyExistsException("Email is already in use.");
        }

        InitUser(ref userEntity);
        
        userEntity.PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerUserDto.Password);
        
        _applicationContext.Users.Add(userEntity);
        await _applicationContext.SaveChangesAsync();

        return _mapper.Map<UserDTO>(userEntity);
    }

    private void InitUser(ref User userEntity)
    {
        userEntity.Posts = new List<Post>();
        userEntity.Avatar = new Image{Url = ""};
        userEntity.FollowersIds = new List<int>();
        userEntity.FollowingIds = new List<int>();
        userEntity.Bio = "";
        userEntity.Location = "";
        userEntity.BookmarkedPostsIds = new List<int>();
        userEntity.RepostsIds = new List<int>();
    }

    public virtual async Task PutUser(int userId, UserDTO user)
    {
        ValidationResult validationResult = await _userDTOValidator.ValidateAsync(user);
        
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors[0].ErrorMessage);
        }
        
        var userToUpdate = await _userQueryRepository.GetUserById(userId);
        
        if (userToUpdate == null)
        {
            throw new UserNotFoundException("User with specified id does not exist");
        }

        userToUpdate.Location = user.Location;
        userToUpdate.Bio = user.Bio;

        if (!string.IsNullOrEmpty(user.Avatar?.Url))
        {
            userToUpdate.Avatar!.Url = user.Avatar.Url;
        }
        
        var userEntity = _mapper.Map<User>(userToUpdate);

        _applicationContext.Attach(userEntity);
        _applicationContext.Attach(userEntity.Avatar!);
        _applicationContext.Entry(userEntity).Property(u => u.Location).IsModified = true;
        _applicationContext.Entry(userEntity).Property(u => u.Bio).IsModified = true;

        _applicationContext.Entry(userEntity.Avatar!).Property(img => img.Url).IsModified = true;
        await _applicationContext.SaveChangesAsync();
    }

    public async Task DeleteUser(int id)
    {
        var userModel = await _userQueryRepository.GetUserById(id); 
        var followers = userModel.FollowersIds.Select(async i => await _userQueryRepository.GetUserById(i));
        var followings = userModel.FollowingIds.Select(async i => await _userQueryRepository.GetUserById(i));
        var bookmarkedPosts = userModel.BookmarkedPostsIds.Select(async i => await _postQueryRepository.GetPostById(i));
        var repostedPosts = userModel.RepostsIds.Select(async i => await _postQueryRepository.GetPostById(i));
        
        if (userModel == null)
        {
            throw new UserNotFoundException("User with specified id does not exist");
        }
        _applicationContext.Users.Remove(_mapper.Map<User>(userModel));
        
        
        foreach (var follower in followers)
        {
            IFollower awaitedFollowerEntity = _mapper.Map<User>(await follower);
            awaitedFollowerEntity.FollowingIds.Remove(id);
            _applicationContext.Attach(awaitedFollowerEntity);
            _applicationContext.Entry(awaitedFollowerEntity).Property(u => u.FollowingIds).IsModified = true;
        }
        
        foreach (var following in followings)
        {
            IFollowing awaitedFollowingEntity = _mapper.Map<User>(await following);
            awaitedFollowingEntity.FollowersIds.Remove(id);
            _applicationContext.Attach(awaitedFollowingEntity);
            _applicationContext.Entry(awaitedFollowingEntity).Property(u => u.FollowersIds).IsModified = true;
        }
        
        foreach (var bookmarkedPost in bookmarkedPosts)
        {
            var awaitedBookmarkedPost = await bookmarkedPost;

            awaitedBookmarkedPost.Bookmarks--;

            _applicationContext.Attach(awaitedBookmarkedPost);
            _applicationContext.Entry(awaitedBookmarkedPost).Property(p => p.Bookmarks).IsModified = true;
            await _applicationContext.SaveChangesAsync();
            _applicationContext.ChangeTracker.Clear();
        }
        
        foreach (var repostedPost in repostedPosts)
        {
            var awaitedRepostedPost = await repostedPost;

            awaitedRepostedPost.RepostersIds.Remove(id);

            _applicationContext.Attach(awaitedRepostedPost);
            _applicationContext.Entry(awaitedRepostedPost).Property(p => p.RepostersIds).IsModified = true;
            
            await _applicationContext.SaveChangesAsync();
            _applicationContext.ChangeTracker.Clear();
        }
        
        await _applicationContext.SaveChangesAsync();
    }

    private bool IsSameUser(int userId, int currentUserId) => userId == currentUserId;
}