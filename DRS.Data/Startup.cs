using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DRS.Data.BusinessEntities;
using DRS.Model.Models;

namespace DRS.Data
{
    public static class Startup
    {
        public static void Configuration()
        {
            Mapper.Initialize(cfg => CreateMappings(cfg));
            Mapper.AssertConfigurationIsValid();
        }

        private static void CreateMappings(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<Archive, ArchiveEntity>();
            cfg.CreateMap<DocumentCategory, DocumentCategoryEntity>();
            cfg.CreateMap<Document, DocumentEntity>();
            cfg.CreateMap<DocumentField, DocumentFieldEntity>();
            cfg.CreateMap<DocumentFieldSetting, DocumentFieldSettingEntity>();
            cfg.CreateMap<DocumentRule, DocumentRuleEntity>();
            cfg.CreateMap<DocumentRuleSetting, DocumentRuleSettingEntity>();
            cfg.CreateMap<DocumentSource, DocumentSourceEntity>();
            cfg.CreateMap<ProcessHandler, ProcessHandlerEntity>();
            cfg.CreateMap<ProcessQueue, ProcessQueueEntity>();
            cfg.CreateMap<ProcessSchema, ProcessSchemaEntity>();
            cfg.CreateMap<ProcessSchemaStep, ProcessSchemaStepEntity>();
            cfg.CreateMap<ProcessSchemaStepSetting, ProcessSchemaStepSettingEntity>();
            cfg.CreateMap<SystemSetting, SystemSettingEntity>();

            cfg.CreateMissingTypeMaps = true;
        }
    }
}
