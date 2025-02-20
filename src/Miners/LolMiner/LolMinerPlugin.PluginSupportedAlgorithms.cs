﻿using NHM.Common.Enums;
using NHM.MinerPluginToolkitV1.Configs;
using System.Collections.Generic;
using SAS = NHM.MinerPluginToolkitV1.Configs.PluginSupportedAlgorithmsSettings.SupportedAlgorithmSettings;

namespace LolMiner
{
    public partial class LolMinerPlugin
    {
#if !LHR_BUILD_ON
        const ulong AMD_8GBMemory = 7UL << 30; // 7GB but really 8GB
        const ulong AMD_6GBMemory = 5UL << 30; // 5GB but really 6GB
        const ulong AMD_3GBMemory = 3UL << 30; // 3GB but really 4GB
        internal static List<SAS> SupportedAMDAlgos()
        {
            return new List<SAS>
                    {
                        new SAS(AlgorithmType.GrinCuckatoo31) { NonDefaultRAMLimit = AMD_8GBMemory, Enabled = false},
                        new SAS(AlgorithmType.GrinCuckatoo32) {Enabled = false},
                        new SAS(AlgorithmType.ZHash) {Enabled = false},
                        new SAS(AlgorithmType.BeamV3) { NonDefaultRAMLimit = AMD_3GBMemory },
                        new SAS(AlgorithmType.DaggerHashimoto),
                        new SAS(AlgorithmType.ZelHash),
                    };
        }

        // NVIDIA OpenCL backend is not really that stable
        internal static List<SAS> SupportedNVIDIAOpenCLAlgos(bool enabled = false)
        {
            return new List<SAS>
                    {
                        new SAS(AlgorithmType.GrinCuckatoo31) {Enabled = enabled },
                        new SAS(AlgorithmType.ZelHash),
                    };
        }
#endif
        protected override PluginSupportedAlgorithmsSettings DefaultPluginSupportedAlgorithmsSettings => new PluginSupportedAlgorithmsSettings
        {
            // fixed fee
            DefaultFee = 1.0,
            AlgorithmFeesV2 = new Dictionary<string, double>
            {
                { $"{AlgorithmType.DaggerHashimoto}", 0.7 }
            },
#if LHR_BUILD_ON
            Algorithms = new Dictionary<DeviceType, List<SAS>>
            {
                {
                    DeviceType.NVIDIA,
                    new List<SAS>
                    {
                        new SAS(AlgorithmType.DaggerHashimoto),
                    }
                }
            }
#else
            Algorithms = new Dictionary<DeviceType, List<SAS>>
            {
                // don't use NVIDIA OpenCL backend
                //{
                //    DeviceType.NVIDIA,
                //    SupportedNVIDIAOpenCLAlgos(false) // dsable NVIDIA by default
                //},
                {
                    DeviceType.NVIDIA,
                    new List<SAS>
                    {

                        new SAS(AlgorithmType.GrinCuckatoo31),
                        new SAS(AlgorithmType.GrinCuckatoo32),
                        new SAS(AlgorithmType.ZHash),
                        new SAS(AlgorithmType.BeamV3),
                        new SAS(AlgorithmType.DaggerHashimoto),
                        new SAS(AlgorithmType.ZelHash),
                    }
                },
                {
                    DeviceType.AMD,
                    SupportedAMDAlgos()
                }
            }
#endif
        };
    }
}
