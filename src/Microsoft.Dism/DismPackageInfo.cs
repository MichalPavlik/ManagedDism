﻿using Microsoft.Win32;
using System;
using System.Runtime.InteropServices;

namespace Microsoft.Dism
{
    public static partial class DismApi
    {
        /// <summary>
        /// Describes detailed package information such as the client used to install the package, the date and time that the package was installed, and support information.
        /// </summary>
        /// <remarks>
        /// <a href="http://msdn.microsoft.com/en-us/library/windows/desktop/hh824774.aspx"/>
        /// typedef struct _DismPackageInfo
        /// {
        ///     PCWSTR PackageName;
        ///     DismPackageFeatureState PackageState;
        ///     DismReleaseType ReleaseType;
        ///     SYSTEMTIME InstallTime;
        ///     BOOL Applicable;
        ///     PCWSTR Copyright;
        ///     PCWSTR Company;
        ///     SYSTEMTIME CreationTime;
        ///     PCWSTR DisplayName;
        ///     PCWSTR Description;
        ///     PCWSTR InstallClient;
        ///     PCWSTR InstallPackageName;
        ///     SYSTEMTIME LastUpdateTime;
        ///     PCWSTR ProductName;
        ///     PCWSTR ProductVersion;
        ///     DismRestartType RestartRequired;
        ///     DismFullyOfflineInstallableType FullyOffline;
        ///     PCWSTR SupportInformation;
        ///     DismCustomProperty* CustomProperty;
        ///     UINT CustomPropertyCount;
        ///     DismFeature* Feature;
        ///     UINT FeatureCount;
        /// } DismPackageInfo;
        /// </remarks>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 4)]
        internal struct DismPackageInfo_
        {
            /// <summary>
            /// The name of the package.
            /// </summary>
            public string PackageName;

            /// <summary>
            /// A DismPackageFeatureState Enumeration value such as DismStateResolved.
            /// </summary>
            public DismPackageFeatureState PackageState;

            /// <summary>
            /// A DismReleaseType Enumeration value such as DismReleaseTypeUpdate.
            /// </summary>
            public DismReleaseType ReleaseType;

            /// <summary>
            /// The date and time that the package was installed. This field is local time, based on the servicing host computer.
            /// </summary>
            public SYSTEMTIME InstallTime;

            /// <summary>
            /// TRUE if the package is applicable to the image, otherwise FALSE.
            /// </summary>
            public bool Applicable;

            /// <summary>
            /// The copyright information of the package.
            /// </summary>
            public string Copyright;

            /// <summary>
            /// The company that released the package.
            /// </summary>
            public string Company;

            /// <summary>
            /// The date and time that the package was created. This field is local time, based on the time zone of the computer that created the package.
            /// </summary>
            public SYSTEMTIME CreationTime;

            /// <summary>
            /// The display name of the package.
            /// </summary>
            public string DisplayName;

            /// <summary>
            /// A description of the purpose of the package.
            /// </summary>
            public string Description;

            /// <summary>
            /// The client that installed this package.
            /// </summary>
            public string InstallClient;

            /// <summary>
            /// The original file name used for the package during installation.
            /// </summary>
            public string InstallPackageName;

            /// <summary>
            /// The date and time when this package was last updated. This field is local time, based on the servicing host computer.
            /// </summary>
            public SYSTEMTIME LastUpdateTime;

            /// <summary>
            /// The product name for this package.
            /// </summary>
            public string ProductName;

            /// <summary>
            /// The product version for this package.
            /// </summary>
            public string ProductVersion;

            /// <summary>
            /// A DismRestartType Enumeration value describing whether a restart is required after installing the package on an online image
            /// </summary>
            public DismRestartType RestartRequired;

            /// <summary>
            /// A DismFullyOfflineInstallableType Enumeration value describing whether a package can be installed offline without booting the image.
            /// </summary>
            public DismFullyOfflineInstallableType FullyOffline;

            /// <summary>
            /// A string listing additional support information for this package.
            /// </summary>
            public string SupportInformation;

            /// <summary>
            /// An array of DismCustomProperty Structure objects representing the custom properties of the package.
            /// </summary>
            public IntPtr CustomProperty;

            /// <summary>
            /// The number of elements in the CustomProperty array.
            /// </summary>
            public UInt32 CustomPropertyCount;

            /// <summary>
            /// An array of DismFeature Structure objects representing the features in the package.
            /// </summary>
            public IntPtr Feature;

            /// <summary>
            /// The number of elements in the Feature array.
            /// </summary>
            public UInt32 FeatureCount;
        }
    }

    /// <summary>
    /// Represents detailed package information such as the client used to install the package, the date and time that the package was installed, and support information.
    /// </summary>
    public sealed class DismPackageInfo
    {
        private readonly DismCustomPropertyCollection _customProperties = new DismCustomPropertyCollection();
        private readonly DismFeatureCollection _features = new DismFeatureCollection();
        private readonly DismApi.DismPackageInfo_ _packageInfo;

        /// <summary>
        /// Initializes a new instance of the DismPackageInfo class.
        /// </summary>
        /// <param name="packageInfoPtr">A pointer to a native <see cref="DismApi.DismPackageInfo_"/> struct.</param>
        internal DismPackageInfo(IntPtr packageInfoPtr)
            : this(packageInfoPtr.ToStructure<DismApi.DismPackageInfo_>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the DismPackageInfo class.
        /// </summary>
        /// <param name="packageInfo">A <see cref="DismApi.DismPackageInfo_"/> struct containing data for this object.</param>
        internal DismPackageInfo(DismApi.DismPackageInfo_ packageInfo)
        {
            _packageInfo = packageInfo;

            // See if there are any custom properties
            if (_packageInfo.CustomPropertyCount > 0 && _packageInfo.CustomProperty != IntPtr.Zero)
            {
                // Add the items
                _customProperties.AddRange<DismApi.DismCustomProperty_>(_packageInfo.CustomProperty, (int)_packageInfo.CustomPropertyCount, i => new DismCustomProperty(i));
            }

            // See if there are any features associated with the package
            if (_packageInfo.FeatureCount > 0 && _packageInfo.Feature != IntPtr.Zero)
            {
                // Add the items
                _features.AddRange<DismApi.DismFeature_>(_packageInfo.Feature, (int)_packageInfo.FeatureCount, i => new DismFeature(i));
            }
        }

        /// <summary>
        /// Gets a value indicating if the package is applicable to the image.
        /// </summary>
        public bool Applicable => _packageInfo.Applicable;

        /// <summary>
        /// The company that released the package.
        /// </summary>
        public string Company => _packageInfo.Company;

        /// <summary>
        /// The copyright information of the package.
        /// </summary>
        public string Copyright => _packageInfo.Copyright;

        /// <summary>
        /// The date and time that the package was created. This field is local time, based on the time zone of the computer that created the package.
        /// </summary>
        public DateTime CreationTime => _packageInfo.CreationTime;

        /// <summary>
        /// An array of DismCustomProperty Structure objects representing the custom properties of the package.
        /// </summary>
        public DismCustomPropertyCollection CustomProperties => _customProperties;

        /// <summary>
        /// A description of the purpose of the package.
        /// </summary>
        public string Description => _packageInfo.Description;

        /// <summary>
        /// The display name of the package.
        /// </summary>
        public string DisplayName => _packageInfo.DisplayName;

        /// <summary>
        /// An array of DismFeature Structure objects representing the features in the package.
        /// </summary>
        public DismFeatureCollection Features => _features;

        /// <summary>
        /// A DismFullyOfflineInstallableType Enumeration value describing whether a package can be installed offline without booting the image.
        /// </summary>
        public DismFullyOfflineInstallableType FullyOffline => _packageInfo.FullyOffline;

        /// <summary>
        /// The client that installed this package.
        /// </summary>
        public string InstallClient => _packageInfo.InstallClient;

        /// <summary>
        /// The original file name used for the package during installation.
        /// </summary>
        public string InstallPackageName => _packageInfo.InstallPackageName;

        /// <summary>
        /// The date and time that the package was installed.
        /// </summary>
        public DateTime InstallTime => _packageInfo.InstallTime;

        /// <summary>
        /// The date and time when this package was last updated. This field is local time, based on the servicing host computer.
        /// </summary>
        public DateTime LastUpdateTime => _packageInfo.LastUpdateTime;

        /// <summary>
        /// The name of the package.
        /// </summary>
        public string PackageName => _packageInfo.PackageName;

        /// <summary>
        /// The state of the package.
        /// </summary>
        public DismPackageFeatureState PackageState => _packageInfo.PackageState;

        /// <summary>
        /// The product name for this package.
        /// </summary>
        public string ProductName => _packageInfo.ProductName;

        /// <summary>
        /// The product version for this package.
        /// </summary>
        public string ProductVersion => _packageInfo.ProductVersion;

        /// <summary>
        /// The release type of the package.
        /// </summary>
        public DismReleaseType ReleaseType => _packageInfo.ReleaseType;

        /// <summary>
        /// A DismRestartType Enumeration value describing whether a restart is required after installing the package on an online image
        /// </summary>
        public DismRestartType RestartRequired => _packageInfo.RestartRequired;

        /// <summary>
        /// A string listing additional support information for this package.
        /// </summary>
        public string SupportInformation => _packageInfo.SupportInformation;

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            return obj != null && Equals(obj as DismPackageInfo);
        }

        /// <summary>
        /// Determines whether the specified <see cref="DismPackageInfo"/> is equal to the current <see cref="DismPackageInfo"/>.
        /// </summary>
        /// <param name="packageInfo">The <see cref="DismPackageInfo"/> object to compare with the current object.</param>
        /// <returns>true if the specified <see cref="DismPackageInfo"/> is equal to the current <see cref="DismPackageInfo"/>; otherwise, false.</returns>
        public bool Equals(DismPackageInfo packageInfo)
        {
            return packageInfo != null
                   && DisplayName == packageInfo.DisplayName;
        }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>A hash code for the current <see cref="T:System.Object"/>.</returns>
        public override int GetHashCode()
        {
            return (String.IsNullOrEmpty(DisplayName) ? 0 : DisplayName.GetHashCode());
        }
    }
}