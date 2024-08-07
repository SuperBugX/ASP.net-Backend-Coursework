using FootballAPI.Services;
using System.Collections.Generic;
using Xunit;

namespace FootBallAPITestProject
{
    public class ModifiersAPITests
    {

        [Fact]
        public void API_Returns_Modifiers()
        {
            //ARRANGE
            MemoryCacheManager cacheManager = new MemoryCacheManager();
            FootballModifiersService service = new FootballModifiersService(cacheManager);

            //ACT
            Dictionary<string, string> modifiers = service.GetDivisionModifiersAsync().Result;

            //ASSERT
            Assert.IsType<Dictionary<string, string>>(modifiers);
        }
    }
}
