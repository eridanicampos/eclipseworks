using AutoMapper;
using ProjectTest.Application.DTO;
using ProjectTest.Application.DTO.Comentarios;
using ProjectTest.Application.DTO.Projetos;
using ProjectTest.Application.DTO.User;
using ProjectTest.Domain.Entities;

namespace ProjectTest.Application.AutoMapper
{
    public class AutoMapperSetup : Profile
    {
        public AutoMapperSetup()
        {
            #region DTOtoDomain

            CreateMap<UserDTO, Usuario>();
            CreateMap<UserModifyDTO, Usuario>();
            CreateMap<UserParmersDTO, Usuario>();
            CreateMap<ProjetoDto, Projeto>();
            CreateMap<TarefaDto, Tarefa>();
            CreateMap<ComentarioDTO, Comentario>();
            CreateMap<UpdateProjetoDto, Projeto>();

            CreateMap<CreateProjetoDto, Projeto>();

            CreateMap<CreateTarefaDto, Tarefa>()
                .ConstructUsing(dto => new Tarefa(dto.Prioridade))
                .ForMember(dest => dest.Titulo, opt => opt.MapFrom(src => src.Titulo))
                .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.Descricao))
                .ForMember(dest => dest.DataVencimento, opt => opt.MapFrom(src => src.DataVencimento))
                .ForMember(dest => dest.Comentarios, opt => opt.MapFrom(src => src.Comentarios));

            CreateMap<CreateComentarioDto, Comentario>();
            CreateMap<UpdateComentarioDto, Comentario>();


            CreateMap<UpdateTarefaDto, Tarefa>()
                .ForMember(dest => dest.Prioridade, opt => opt.Ignore())
                .ForMember(dest => dest.Comentarios, opt => opt.MapFrom(src => src.Comentarios))
                .ForMember(dest => dest.UsuarioId, opt => opt.MapFrom(src => src.UsuarioId));


            #endregion

            #region DomaintoDTO

            CreateMap<Usuario, UserDTO>();
            CreateMap<Usuario, UserModifyDTO>();
            CreateMap<Usuario, UserParmersDTO>();

            CreateMap<Projeto, ProjetoDto>()
                .ForMember(dest => dest.Tarefas, opt => opt.MapFrom(src => src.Tarefas)); // Se ProjetoDto tiver a lista

            CreateMap<Tarefa, TarefaDto>()
                .ForMember(dest => dest.NomeProjeto, opt => opt.MapFrom(src => src.Projeto.Nome))
                .ForMember(dest => dest.Comentarios, opt => opt.MapFrom(src => src.Comentarios));

            CreateMap<Comentario, ComentarioDTO>();
            #endregion

        }
    }
}
