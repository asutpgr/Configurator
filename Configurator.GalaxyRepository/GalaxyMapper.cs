using ArchestrA.GRAccess;
using Configurator.Model;
namespace Configurator.GalaxyRepository
{
    public static class GalaxyMapper
    { 
        public static MxDataType GetMxDataType(GDataTypeEnum type)
        {
            switch (type)
            {
                #region Соответствие типов GDataTypeEnum -> MxDataType
                case GDataTypeEnum.Boolean:
                    return MxDataType.MxBoolean;
                case GDataTypeEnum.Integer:
                    return MxDataType.MxInteger;
                case GDataTypeEnum.Float:
                    return MxDataType.MxFloat;
                case GDataTypeEnum.Double:
                    return MxDataType.MxDouble;
                case GDataTypeEnum.String:
                    return MxDataType.MxString;
                case GDataTypeEnum.NoData:
                    return MxDataType.MxNoData;
                case GDataTypeEnum.Time:
                    return MxDataType.MxTime;
                case GDataTypeEnum.ElapsedTime:
                    return MxDataType.MxElapsedTime;
                case GDataTypeEnum.ReferenceType:
                    return MxDataType.MxReferenceType;
                case GDataTypeEnum.StatusType:
                    return MxDataType.MxStatusType;
                case GDataTypeEnum.DataTypeEnum:
                    return MxDataType.MxDataTypeEnum;
                case GDataTypeEnum.SecurityClassificationEnum:
                    return MxDataType.MxSecurityClassificationEnum;
                case GDataTypeEnum.DataQualityType:
                    return MxDataType.MxDataQualityType;
                case GDataTypeEnum.QualifiedEnum:
                    return MxDataType.MxQualifiedEnum;
                case GDataTypeEnum.QualifiedStruct:
                    return MxDataType.MxQualifiedStruct;
                case GDataTypeEnum.InternationalizedString:
                    return MxDataType.MxInternationalizedString;
                case GDataTypeEnum.BigString:
                    return MxDataType.MxBigString;
                case GDataTypeEnum.DataTypeEND:
                    return MxDataType.MxDataTypeEND;
                case GDataTypeEnum.Unknown:
                    return MxDataType.MxDataTypeUnknown;
                default:
                    return MxDataType.MxDataTypeUnknown;
                    #endregion
            }
        }
        public static GDataTypeEnum GetGDataType(MxDataType type)
        {
            switch (type)
            {
                #region Соответствие типов MxDataType -> GDataTypeEnum
                case MxDataType.MxDataTypeUnknown:
                    return GDataTypeEnum.Unknown;
                case MxDataType.MxNoData:
                    return GDataTypeEnum.NoData;
                case MxDataType.MxBoolean:
                    return GDataTypeEnum.Boolean;
                case MxDataType.MxInteger:
                    return GDataTypeEnum.Integer;
                case MxDataType.MxFloat:
                    return GDataTypeEnum.Float;
                case MxDataType.MxDouble:
                    return GDataTypeEnum.Double;
                case MxDataType.MxString:
                    return GDataTypeEnum.String;
                case MxDataType.MxTime:
                    return GDataTypeEnum.Time;
                case MxDataType.MxElapsedTime:
                    return GDataTypeEnum.ElapsedTime;
                case MxDataType.MxReferenceType:
                    return GDataTypeEnum.ReferenceType;
                case MxDataType.MxStatusType:
                    return GDataTypeEnum.StatusType;
                case MxDataType.MxDataTypeEnum:
                    return GDataTypeEnum.DataTypeEnum;
                case MxDataType.MxSecurityClassificationEnum:
                    return GDataTypeEnum.SecurityClassificationEnum;
                case MxDataType.MxDataQualityType:
                    return GDataTypeEnum.DataQualityType;
                case MxDataType.MxQualifiedEnum:
                    return GDataTypeEnum.QualifiedEnum;
                case MxDataType.MxQualifiedStruct:
                    return GDataTypeEnum.QualifiedStruct;
                case MxDataType.MxInternationalizedString:
                    return GDataTypeEnum.InternationalizedString;
                case MxDataType.MxBigString:
                    return GDataTypeEnum.BigString;
                case MxDataType.MxDataTypeEND:
                    return GDataTypeEnum.DataTypeEND;
                #endregion
                default:
                    return GDataTypeEnum.Unknown;
            }
        }
        public static ECATEGORY GCategory(int number)
        {
            switch (number)
            {
                case 0:
                    return ECATEGORY.idxCategoryUndefined;
                case 1:
                    return ECATEGORY.idxCategoryPlatformEngine;
                case 2:
                    return ECATEGORY.idxCategoryClusterEngine;
                case 3:
                    return ECATEGORY.idxCategoryApplicationEngine;
                case 4:
                    return ECATEGORY.idxCategoryViewEngine;
                case 5:
                    return ECATEGORY.idxCategoryProductEngine;
                case 6:
                    return ECATEGORY.idxCategoryHistoryEngine;
                case 7:
                    return ECATEGORY.idxCategoryPrintEngine;
                case 8:
                    return ECATEGORY.idxCategoryOutpost;
                case 9:
                    return ECATEGORY.idxCategoryQueryEngine;
                case 10:
                    return ECATEGORY.idxCategoryApplicationObject;
                case 11:
                    return ECATEGORY.idxCategoryIONetwork;
                case 12:
                    return ECATEGORY.idxCategoryIODevice;
                case 13:
                    return ECATEGORY.idxCategoryArea;
                case 14:
                    return ECATEGORY.idxCategoryUserProfile;
                case 15:
                    return ECATEGORY.idxCategoryDisplay;
                case 16:
                    return ECATEGORY.idxCategorySymbol;
                case 17:
                    return ECATEGORY.idxCategoryViewApp;
                case 18:
                    return ECATEGORY.idxCategoryProductionObject;
                case 19:
                    return ECATEGORY.idxCategoryReport;
                case 20:
                    return ECATEGORY.idxCategorySharedProcedure;
                case 21:
                    return ECATEGORY.idxCategoryInsertablePrimitive;
                case 22:
                    return ECATEGORY.idxCategoryIDEMacro;
                case 23:
                    return ECATEGORY.idxCategoryGalaxy;
                default:
                    return ECATEGORY.idxCategoryUndefined;

            }
        }

    }
}
