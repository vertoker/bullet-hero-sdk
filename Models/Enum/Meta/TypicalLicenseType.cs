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
        /// <summary> License with full freedom of usage (https://choosealicense.com/licenses/unlicense/) </summary>
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
        
        // Allows to share/remix/commercial
        
        /// <summary> Creative Commons Zero v4.0 - public domain </summary>
        CC0_4_0 = 100,
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
        
        
        // Commercial music licenses for non-commercial games
        // NCS = 150,
        // SoundBible = 151,
        // DL_Sounds = 152,
    }
}