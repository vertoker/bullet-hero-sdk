// ReSharper disable InconsistentNaming
namespace BHSDK.Models.Enum.Meta
{
    public enum TypicalLicenseType : short
    {
        // -----------------------------------------------------------------------------------------------------------
        // General licenses (text / bytes / mostly open source)
        // -----------------------------------------------------------------------------------------------------------
        
        /// <summary> All rights reserved </summary>
        Proprietary = 0,
        /// <summary> No rights reserved, full freedom of usage (https://choosealicense.com/licenses/unlicense/) </summary>
        PublicDomain = 1,
        
        /// <summary> MIT License (Massachusetts Institute of Technology)
        /// (https://choosealicense.com/licenses/mit/) </summary> 
        MIT = 2,
        
        /// <summary> GNU General Public License v3.0
        /// (https://choosealicense.com/licenses/gpl-3.0/) </summary>
        GPL_3_0 = 3,
        
        /// <summary> GNU Affero General Public License v3.0
        /// (https://choosealicense.com/licenses/agpl-3.0/) </summary>
        AGPL_3_0 = 4,
        
        /// <summary> GNU Lesser General Public License v3.0
        /// (https://choosealicense.com/licenses/lgpl-3.0/) </summary>
        LGPL_3_0 = 5,
        
        /// <summary> Apache v2.0 (https://www.apache.org/licenses/LICENSE-2.0) </summary>
        Apache_2_0 = 6,
        
        /// <summary> SIL OFL (Open Font License) v1.1
        /// (https://openfontlicense.org/open-font-license-official-text/) </summary>
        SIL_OFL_1_1 = 7,
        
        // -----------------------------------------------------------------------------------------------------------
        // Media licenses (audio / images)
        // -----------------------------------------------------------------------------------------------------------
        
        // Creative Commons Family
        // https://creativecommons.org/cc-licenses/
        
        /// <summary> Creative Commons Zero v4.0 - public domain
        /// (https://creativecommons.org/publicdomain/zero/1.0/) </summary>
        CC0_1_0 = 100,
        
        // v4.0
        
        // Allows to share/remix/commercial
        
        /// <summary> Creative Commons Attribution v4.0
        /// (https://creativecommons.org/licenses/by/4.0/) </summary>
        CC_BY_4_0 = 101,
        /// <summary> Creative Commons Attribution-ShareAlike v4.0
        /// (https://creativecommons.org/licenses/by-sa/4.0/) </summary>
        CC_BY_SA_4_0 = 102,
        
        // Allows to share/remix
        
        /// <summary> Creative Commons Attribution-NonCommercial v4.0
        /// (https://creativecommons.org/licenses/by-nc/4.0/) </summary>
        CC_BY_NC_4_0 = 103,
        /// <summary> Creative Commons Attribution-NonCommercial-ShareAlike v4.0
        /// (https://creativecommons.org/licenses/by-nc-sa/4.0/) </summary>
        CC_BY_NC_SA_4_0 = 104,
        
        // Allows to share
        
        /// <summary> Creative Commons Attribution-NoDerivation v4.0
        /// (https://creativecommons.org/licenses/by-nd/4.0/) </summary>
        CC_BY_ND_4_0 = 105,
        /// <summary> Creative Commons Attribution-NonCommercial-NoDerivation v4.0
        /// (https://creativecommons.org/licenses/by-nc-nd/4.0/) </summary>
        CC_BY_NC_ND_4_0 = 106,
        
        // v3.0
        
        // Allows to share/remix/commercial
        
        /// <summary> Creative Commons Attribution v3.0
        /// (https://creativecommons.org/licenses/by/3.0/) </summary>
        CC_BY_3_0 = 107,
        /// <summary> Creative Commons Attribution-ShareAlike v3.0
        /// (https://creativecommons.org/licenses/by-sa/3.0/) </summary>
        CC_BY_SA_3_0 = 108,
        
        // Allows to share/remix
        
        /// <summary> Creative Commons Attribution-NonCommercial v3.0
        /// (https://creativecommons.org/licenses/by-nc/3.0/) </summary>
        CC_BY_NC_3_0 = 109,
        /// <summary> Creative Commons Attribution-NonCommercial-ShareAlike v3.0
        /// (https://creativecommons.org/licenses/by-nc-sa/3.0/) </summary>
        CC_BY_NC_SA_3_0 = 110,
        
        // Allows to share
        
        /// <summary> Creative Commons Attribution-NoDerivation v3.0
        /// (https://creativecommons.org/licenses/by-nd/3.0/) </summary>
        CC_BY_ND_3_0 = 111,
        /// <summary> Creative Commons Attribution-NonCommercial-NoDerivation v3.0
        /// (https://creativecommons.org/licenses/by-nc-nd/3.0/) </summary>
        CC_BY_NC_ND_3_0 = 112,
        
        
        // Commercial music licenses for non-commercial games
        
        /// <summary> Standard YouTube License, require author permission
        /// (https://support.google.com/youtube/answer/2797468) </summary>
        YT = 150,
        
        // NCS = 150,
        // SoundBible = 151,
        // DL_Sounds = 152,
    }
}