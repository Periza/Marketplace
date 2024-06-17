using Marketplace.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Api;

[Route(template: "/ad")]
public class ClassifiedAdsCommandsApi : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Post(ClassifiedAds.V1.Create request)
    {
        // handle request here
        return Ok();
    }
    
}