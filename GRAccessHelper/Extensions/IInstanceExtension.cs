using System;
using System.Threading.Tasks;

namespace GRAccessHelper.Extensions
{
    using ArchestrA.GRAccess;
    using Exceptions;
   public static class IInstanceExtension
    {
        // Деплоим экземпляр асинхронно
        public async static Task DeployAsync(this IInstance inst)
        {
            if (inst == null)
                throw new ArgumentNullException(nameof(inst));
           await Task.Run(() =>
           {
               inst.Deploy(
                   EActionForCurrentlyDeployedObjects.redeployOriginal,
                   ESkipIfCurrentlyUndeployed.dontSkipIfCurrentlyUndeployed,
                   EDeployOnScan.doDeployOnScan,
                   EForceOffScan.doForceOffScan,
                   ECascade.dontCascade);
           });
            if (!inst.CommandResult.Successful)
                GalaxyExceptions.ThrowIfNoSuccess(inst.CommandResult);
        }
        // Андеплоим экземпляр асинхронно
        public async static Task UnDeployAsync(this IInstance inst)
        {
            if (inst == null)
                throw new ArgumentNullException(nameof(inst));
            await Task.Run(() =>
            {
                inst.Undeploy(
                    EForceOffScan.doForceOffScan,
                    ECascade.doCascade);
            });
            if (!inst.CommandResult.Successful)
                GalaxyExceptions.ThrowIfNoSuccess(inst.CommandResult);
        }
        //Получаем сообщение о последней ошибке
        public static string GetFailMsg(this IInstance inst)
        {
            if (inst == null)
                throw new ArgumentNullException(nameof(inst));
            if (inst.CommandResult == null || inst.CommandResult.Successful)
                return null;
            else
                return $"{inst.CommandResult.Text}:{inst.CommandResult.CustomMessage}";
        }



    }
}
