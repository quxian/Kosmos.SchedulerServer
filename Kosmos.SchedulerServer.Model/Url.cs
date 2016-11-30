using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kosmos.SchedulerServer.Model {
    public class Url {
        public string HashCode { get; set; }
        public string Value { get; set; }
        public bool IsDownloaded { get; set; }
        public int Depth { get; set; }

        public string Domain { get; set; }

        public string Parent { get; set; }

        public override bool Equals(object obj) {
            if (ReferenceEquals(obj, null)) return false;

            if (ReferenceEquals(this, obj)) return true;

            var o = obj as Url;
            return HashCode.Equals(o?.HashCode);
        }

        public override int GetHashCode() {
            return HashCode.GetHashCode();
        }
    }
}
