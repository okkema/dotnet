using AutoFixture;
using AutoFixture.Xunit2;
namespace Okkema.Test;
public class AutoMockDataAttribute : AutoDataAttribute
{
    public AutoMockDataAttribute() 
        :base(() => new Fixture()) {}
}