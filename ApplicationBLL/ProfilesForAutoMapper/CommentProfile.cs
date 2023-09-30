using ApplicationCommon.DTOs.Comment;
using ApplicationCommon.DTOs.Post;
using ApplicationCommon.DTOs.User;
using ApplicationDAL.Entities;
using AutoMapper;

namespace ApplicationBLL.ProfilesForAutoMapper;

public class CommentProfile : Profile
{
    public CommentProfile()
    {
        CreateMap<CommentCreateDTO, CommentDTO>();
        CreateMap<CommentDTO, CommentCreateDTO>();
        CreateMap<CommentDTO, Comment>().AfterMap((src, dest, context) =>
        {
            dest.Author = context.Mapper.Map<UserDTO, User>(src.Author);
            dest.Post = src.Post == null ? null : context.Mapper.Map<PostDTO, Post>(src.Post);
            dest.ParentComment = src.ParentComment == null ? null : context.Mapper.Map<CommentDTO, Comment>(src.ParentComment);
        });
        CreateMap<Comment, CommentDTO>().AfterMap((src, dest, context) =>
        {
            dest.Author = context.Mapper.Map<User, UserDTO>(src.Author);
            dest.Post = src.Post == null ? null : context.Mapper.Map<Post, PostDTO>(src.Post);
            dest.ParentComment = src.ParentComment == null ? null : context.Mapper.Map<Comment, CommentDTO>(src.ParentComment);
        });
    }
}