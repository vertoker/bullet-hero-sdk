using System.Collections.Generic;
using BHSDK.Models.Interfaces.Instances;
using BHSDK.Models.V1.Instances;
using NUnit.Framework;

namespace BulletHeroSDK.Tests.Models.Instances
{
    public class PrefabTests
    {
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructV1()
        {
            var prefabV1 = new PrefabV1();
            
            Assert.True(prefabV1.Instances != null);
            Assert.True(prefabV1.PrefabInstances != null);
        }
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructV1_2()
        {
            var instances = new List<IInstance>();
            var prefabInstances = new List<IPrefabInstance>();
            
            var prefabV1 = new PrefabV1(instances, prefabInstances);
            
            Assert.True(prefabV1.Instances == instances);
            Assert.True(prefabV1.PrefabInstances == prefabInstances);
        }
        
        // other versions add here
        
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructCurrent()
        {
            var currentType = typeof(PrefabV1);
            Assert.True(IPrefab.CreateNew().GetType() == currentType);
            Assert.True(IPrefab.GetModelType == currentType);
        }
    }
}