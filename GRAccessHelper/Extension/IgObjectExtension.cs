using System;

namespace GRAccessHelper.Extension
{
    using ArchestrA.GRAccess;
    using Exceptions;
    public static class IgObjectExtension
    {
        // Проверяем статус редактирования и берем на редактирвоание в противном случае
        public static void CheckOutWithCheckStatus(this IgObject obj)
        {
            if (obj.CheckoutStatus != ECheckoutStatus.notCheckedOut)
                throw new GalaxyObjectAlreadyCheckOutedException(obj.Tagname);
            obj.CheckOut();
        }



    }
}
