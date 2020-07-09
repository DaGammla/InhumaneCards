using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InhumaneCards {
	public class Thread {
		Action threadAction;
		public Thread(Action threadAction) {
			this.threadAction = threadAction;
		}

		public void Start() {
			new Task(threadAction).Start();
		}

		public static void Sleep(int a) {

		}
	}
}
