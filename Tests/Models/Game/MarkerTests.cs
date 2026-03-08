using System.Collections.Generic;
using BHSDK.Models.Interfaces.Game;
using BHSDK.Models.V1.Game;
using BHSDK.Models.V1.Values;
using NUnit.Framework;

namespace BulletHeroSDK.Tests.Models.Game
{
    public class MarkerTests
    {
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructV1()
        {
            var markerV1 = new MarkerV1();
            
            Assert.True(markerV1.Name != null);
            Assert.True(markerV1.Description != null);
            Assert.True(markerV1.Frame == 0);
            Assert.True(markerV1.Color != null);
        }
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructV1_2()
        {
            const string name = "Marker Test";
            const string description = "Marker Description";
            const int frame = 12346;
            var color = new ColorV1(1f, 1f, 1f, 1f);
            
            var markerV1 = new MarkerV1(name, description, frame, color);
            
            Assert.True(markerV1.Name == name);
            Assert.True(markerV1.Description == description);
            Assert.True(markerV1.Frame == frame);
            Assert.True(markerV1.Color == color);
        }
        
        // other versions add here
        
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructCurrent()
        {
            var currentType = typeof(MarkerV1);
            Assert.True(IMarker.CreateNew().GetType() == currentType);
            Assert.True(IMarker.GetModelType == currentType);
        }
    }
}