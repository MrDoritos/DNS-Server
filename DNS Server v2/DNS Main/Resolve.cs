using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNS_Server_v2
{
    class Resolve
    {
        private List<Record> DNRecord;

        public Resolve(List<Record> records) { DNRecord = records; }

        public Record ResolveRecord(Record question)
        {
            var matchingTopLevel = (DNRecord.Where(n => n.TopLevelDomain == question.TopLevelDomain));
            if (matchingTopLevel.Count() < 1) { return null; }
            return (matchingTopLevel.FirstOrDefault(n => n.FQDN == question.FQDN));            
        }

        
    }
}
