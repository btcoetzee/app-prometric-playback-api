using System;
using System.Collections.Generic;
using System.Text;

namespace Prometric.Playback.Application.Exceptions
{
    public class AMSOutputAssetAlreadyExistsException : AppException
    {
        public override string Code { get; } = "AMS_output_asset_already_exists";

        public AMSOutputAssetAlreadyExistsException (string assetId) : base($"The Azure Media Services Output Asset {assetId} already exists")
        {            
        }
    }
}
