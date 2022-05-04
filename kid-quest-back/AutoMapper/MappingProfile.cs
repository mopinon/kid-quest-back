using AutoMapper;
using KidQquest.Models;
using KidQquest.Params;
using KidQquest.Params.AnswerVariantParams;
using KidQquest.Params.AttemptParams;
using KidQquest.Params.AwardParams;
using KidQquest.Params.FactParams;
using KidQquest.Params.QuestCategoryParams;
using KidQquest.Params.QuestionParams;
using KidQquest.Params.QuestionTypeParams;
using KidQquest.Params.QuestParams;
using KidQquest.Params.UserAwardParams;

namespace KidQquest.AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateQuestRequest, QuestModel>().ReverseMap();
        CreateMap<UpdateQuestRequest, QuestModel>().ReverseMap();
        CreateMap<QuestDto, QuestModel>().ReverseMap();
        CreateMap<CreateQuestCategoryRequest, QuestCategoryModel>().ReverseMap();
        CreateMap<QuestCategoryDto, QuestCategoryModel>().ReverseMap();
        CreateMap<CreateQuestRequest, QuestCategoryModel>().ReverseMap();
        CreateMap<UpdateQuestRequest, QuestCategoryModel>().ReverseMap();
        CreateMap<CreateQuestionRequest, QuestionModel>().ReverseMap();
        CreateMap<UpdateQuestionRequest, QuestionModel>().ReverseMap();
        CreateMap<QuestionDto, QuestionModel>().ReverseMap();
        CreateMap<CreateQuestionTypeRequest, QuestionTypeModel>().ReverseMap();
        CreateMap<UpdateFactRequest, FactModel>().ReverseMap();
        CreateMap<FactDto, FactModel>().ReverseMap();
        CreateMap<CreateFactRequest, FactModel>().ReverseMap();
        CreateMap<CreateAnswerVariantRequest, AnswerVariantModel>().ReverseMap();
        CreateMap<UpdateAnswerVariantRequest, AnswerVariantModel>().ReverseMap();
        CreateMap<AnswerVariantDto, AnswerVariantModel>().ReverseMap();
        CreateMap<AttemptDto, AttemptModel>().ReverseMap();
        CreateMap<CreateAttemptRequest, AttemptModel>().ReverseMap();
        CreateMap<CreateAwardTypeRequest, AwardTypeModel>().ReverseMap();
        CreateMap<AwardTypeDto, AwardTypeModel>().ReverseMap();
        CreateMap<CreateUserAwardRequest, UserAwardModel>().ReverseMap();
    }
}