using System;

namespace GRAccessHelper.Extensions
{
    using ArchestrA.GRAccess;
    using Exceptions;
    public static class IgObjectExtension
    {
        // Проверяем статус редактирования и берем на редактирвоание в противном случае
        public static void CheckOutWithCheckStatus(this IgObject obj)
        {
            if (obj.IsCheckOuted())
                throw new GalaxyObjectAlreadyCheckOutedException(obj.Tagname);
            obj.CheckOut();
        }
        public static void SaveAndCheckIn(this IgObject obj, string comment = null)
        {
            obj.Save();
            if (comment == null)
                obj.CheckIn(comment);
            else obj.CheckIn();
        }
        // Проверяет, редактируется ли объект( true - редактирутся)
        public static bool IsCheckOuted(this IgObject obj)
        {
            return obj.CheckoutStatus != ECheckoutStatus.notCheckedOut ? true : false;
        }





    }
}
