using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;

namespace Common.Utils
{
    public class GroupHelper
    {
        public static List<Element> GetMembersInGroupRecursive(Document doc, Group group, List<Element> allMembers = null)
        {
            if (group == null)
            {
                return new List<Element>();
            }

            if (allMembers == null)
            {
                allMembers = new List<Element>();
            }

            List<Element> members = group.GetMemberIds().Select(q => doc.GetElement(q)).ToList();

            foreach (Element member in members)
            {
                if (member.GetType() == typeof(Group))
                {
                    GetMembersInGroupRecursive(doc, member as Group, allMembers);
                    continue;
                }

                allMembers.Add(member);
            }

            return allMembers;
        }
    }
}
