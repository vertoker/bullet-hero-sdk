using System.Collections.Generic;
using BHSDK.Models.Interfaces.Instances;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.V1;
using BHSDK.Models.V1.Instances;
using NUnit.Framework;

namespace BulletHeroSDK.Tests.Models.Instances
{
    public class PrefabInstanceTests
    {
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructV1()
        {
            var prefabInstanceV1 = new PrefabInstanceV1();
            
            Assert.True(prefabInstanceV1.SourcePrefab != null);
            Assert.True(prefabInstanceV1.Modifications != null);
        }
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructV1_2()
        {
            var sourcePrefab = new PrefabV1();
            var modifications = new List<IModification>();
            
            var prefabInstanceV1 = new PrefabInstanceV1(sourcePrefab, modifications);
            
            Assert.True(prefabInstanceV1.SourcePrefab == sourcePrefab);
            Assert.True(prefabInstanceV1.Modifications == modifications);
        }
        
        // other versions add here
        
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructCurrent()
        {
            var currentType = typeof(PrefabInstanceV1);
            Assert.True(IPrefabInstance.CreateNew().GetType() == currentType);
            Assert.True(IPrefabInstance.GetModelType == currentType);
        }
    }
}