using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XueQiaoFoundation.Shared.Helper.WeakDelegate
{
    /// <summary>
    /// Represents a reference to a <see cref="Action"/> that may contain a
    /// <see cref="WeakReference"/> to the target.
    /// </summary>
    public class ActionDelegateReference
    {
        private readonly DelegateReference delegateReference;

        public ActionDelegateReference(Action action, bool keepReferenceAlive)
        {
            delegateReference = new DelegateReference(action, keepReferenceAlive);
        }

        public Action Target
        {
            get
            {
                return delegateReference.Target as Action;
            }
        }
    }

    /// <summary>
    /// Represents a reference to a <see cref="Action<T>"/> that may contain a
    /// <see cref="WeakReference"/> to the target.
    /// </summary>
    public class ActionDelegateReference<T>
    {
        private readonly DelegateReference delegateReference;

        public ActionDelegateReference(Action<T> action, bool keepReferenceAlive)
        {
            delegateReference = new DelegateReference(action, keepReferenceAlive);
        }

        public Action<T> Target
        {
            get
            {
                return delegateReference.Target as Action<T>;
            }
        }
    }

    /// <summary>
    /// Represents a reference to a <see cref="Action<T1, T2>"/> that may contain a
    /// <see cref="WeakReference"/> to the target.
    /// </summary>
    public class ActionDelegateReference<T1, T2>
    {
        private readonly DelegateReference delegateReference;

        public ActionDelegateReference(Action<T1, T2> action, bool keepReferenceAlive)
        {
            delegateReference = new DelegateReference(action, keepReferenceAlive);
        }

        public Action<T1, T2> Target
        {
            get
            {
                return delegateReference.Target as Action<T1, T2>;
            }
        }
    }
}
