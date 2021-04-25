using lib.xqclient_base.logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XueQiaoFoundation.Shared.Interface;

namespace XueQiaoFoundation.Shared.ControllerBase
{
    public enum SwitchablePageControllerState
    {
        Created = 0,
        HasInitialized = 1,
        HasRuned = 2,
        HasShutdowned = 3
    }

    public class SwitchablePageControllerEmptyImpl : IController
    {
        public virtual void Initialize()
        {
        }

        public virtual void Run()
        {
        }

        public virtual void Shutdown()
        {
        }
    }

    /// <summary>
    /// 可切换（比如tab control 控制的页面）页面控制器的基类。它能保证在正确的生命周期只能使用正确的 <see cref="IController"/> 生命周期方法
    /// </summary>
    public abstract class SwitchablePageControllerBase : SwitchablePageControllerEmptyImpl
    {
        protected SwitchablePageControllerState WorkspaceControllerState { private set; get; } = SwitchablePageControllerState.Created;

        protected SwitchablePageControllerBase() { }

        protected abstract void DoInitialize();

        protected abstract void DoRun();

        protected abstract void DoShutdown();

        sealed public override void Initialize()
        {
            var state = this.WorkspaceControllerState;
            if (state == SwitchablePageControllerState.Created)
            {
                WorkspaceControllerState = SwitchablePageControllerState.HasInitialized;
                this.DoInitialize();
            }
            else
            {
                AppLog.Info($"controller state is {state}, Can't `Initialize` when state != {SwitchablePageControllerState.Created}");
            }
        }

        sealed public override void Run()
        {
            var state = this.WorkspaceControllerState;
            if (state == SwitchablePageControllerState.HasInitialized)
            {
                WorkspaceControllerState = SwitchablePageControllerState.HasRuned;
                this.DoRun();
            }
            else
            {
                AppLog.Info($"controller state is {state}, Can't `Run` when state != {SwitchablePageControllerState.HasInitialized}");
            }
        }

        sealed public override void Shutdown()
        {
            var state = this.WorkspaceControllerState;
            if (state != SwitchablePageControllerState.HasShutdowned)
            {
                WorkspaceControllerState = SwitchablePageControllerState.HasShutdowned;
                this.DoShutdown();
            }
            else
            {
                AppLog.Info($"controller state is {state}, no need to `Shutdown` again when state == {SwitchablePageControllerState.HasShutdowned}");
            }
        }
    }
}
