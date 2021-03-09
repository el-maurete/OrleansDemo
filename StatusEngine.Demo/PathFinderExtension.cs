using System.Linq;
using StatusEngine.Demo.Model;

namespace StatusEngine.Demo
{
    public static class PathFinderExtension
    {
        public static string Name(this string fullPath) =>
            fullPath.Split('/').Last();
        
        public static string ParentPath(this string fullPath) =>
            string.Join("/", fullPath
                .Split('/')
                .SkipLast(1));

        public static string EventClassGrainPath(this Event eventData) =>
            string.Join("/",
                eventData.AssetGrainPath(),
                eventData.EventClassName);

        public static string AssetGrainPath(this Event eventData) =>
            string.Join("/",
                eventData.OwnerGrainPath(),
                eventData.AssetName);

        public static string OwnerGrainPath(this Event eventData) =>
            eventData.Owner;
    }
}
