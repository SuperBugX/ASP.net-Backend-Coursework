using FootballAPI.Services;
using System;
using Xunit;

namespace FootBallAPITestProject
{
    public class MemoryCacheTests
    {
        [Fact]
        public void Assert_Add_And_Get_Works()
        {
            //ARRANGE
            MemoryCacheManager cacheManager = new MemoryCacheManager("test");

            //ACT
            cacheManager.Add("test", "test");

            //ASSERT
            Assert.Equal("test", cacheManager.Get<String>("test"));
        }

        [Fact]
        public void Assert_Set_And_Get_Works()
        {
            //ARRANGE
            MemoryCacheManager cacheManager = new MemoryCacheManager("test");

            //ACT
            cacheManager.Set("test", "test");

            //ASSERT
            Assert.Equal("test", cacheManager.Get<String>("test"));
        }

        [Fact]
        public void Assert_Add_Expiration_Works()
        {
            //ARRANGE
            MemoryCacheManager cacheManager = new MemoryCacheManager("test");

            //ACT
            cacheManager.Add("test", "test", 3);

            System.Threading.Thread.Sleep(3500);

            //ASSERT
            Assert.True(cacheManager.Get<String>("test") == null);
        }

        [Fact]
        public void Assert_Set_Expiration_Works()
        {
            //ARRANGE
            MemoryCacheManager cacheManager = new MemoryCacheManager("test");

            //ACT
            cacheManager.Set("test", "test", 3);

            System.Threading.Thread.Sleep(3500);

            //ASSERT
            Assert.True(cacheManager.Get<String>("test") == null);
        }

        [Fact]
        public void Assert_Delete_And_Get_Works()
        {
            //ARRANGE
            MemoryCacheManager cacheManager = new MemoryCacheManager("test");

            //ACT
            cacheManager.Add("test", "test");
            cacheManager.Delete("test");

            //ASSERT
            Assert.True(cacheManager.Get<String>("test") == null);
        }
    }
}
