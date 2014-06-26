Option Strict On
Imports System.Collections.Generic
Imports System.Data
Imports System.Web

Public Class Domain
    Protected DataCon As AirArena.Data.DataConnector
#Region " Constructors "
    Public Sub New()
        DataCon = New AirArena.Data.DataConnector
    End Sub
    Public Sub New(ByVal domainCode As Integer)
        Me.New()
        Me.Code = domainCode
        Load(CInt(domainCode))
    End Sub
    Public Sub New(ByVal domain As String)
        Me.New()
        Load(domain)
    End Sub
    Public Sub New(ByVal value As Integer, ByVal findByNum As FindBy)
        Me.New()
        Load(value, findByNum)
    End Sub
#End Region
#Region " Properties "

    Public Enum FindBy
        DomainCode
        CampaignCode
    End Enum

    Private mCode As Integer
    Public Overridable Property Code() As Integer
        Get
            Return mCode
        End Get
        Set(ByVal value As Integer)
            mCode = value
        End Set
    End Property

    Private mCountryCode As Integer
    Public Property CountryCode() As Integer
        Get
            Return mCountryCode
        End Get
        Set(ByVal value As Integer)
            mCountryCode = value
        End Set
    End Property

    Private mCountry As Country
    Public Property Country() As Country
        Get
            If mCountry Is Nothing Then
                mCountry = New Country(mCountryCode)
            End If
            Return mCountry
        End Get
        Set(ByVal value As Country)
            mCountry = value
        End Set
    End Property


    Private mName As String
    Public Overridable Property Name() As String
        Get
            Return mName
        End Get
        Set(ByVal value As String)
            mName = value
        End Set
    End Property

    Private mSupportPhone As String
    Public Overridable Property SupportPhone() As String
        Get
            Return mSupportPhone
        End Get
        Set(ByVal value As String)
            mSupportPhone = value
        End Set
    End Property

    Private mCurrency As String
    Public Overridable Property Currency() As String
        Get
            Return mCurrency
        End Get
        Set(ByVal value As String)
            mCurrency = value
        End Set
    End Property

    Private mCurrencySymbol As String
    Public Overridable Property CurrencySymbol() As String
        Get
            Return mCurrencySymbol
        End Get
        Set(ByVal value As String)
            mCurrencySymbol = value
        End Set
    End Property

    Private mBrandId As Integer
    Public Overridable Property BrandId() As Integer
        Get
            Return mBrandId
        End Get
        Set(ByVal value As Integer)
            mBrandId = value
        End Set
    End Property

    Private mBranding As Branding
    Public Overridable ReadOnly Property Branding() As Branding
        Get
            If mBranding Is Nothing Then
                mBranding = New Branding(BrandId)
            End If
            Return mBranding
        End Get
    End Property

    Private mSecureSite As String
    Public Property SecureSite() As String
        Get
            Return mSecureSite
        End Get
        Set(ByVal value As String)
            mSecureSite = value
        End Set
    End Property
    'Private mBrandingName As String
    'Public Property BrandingName() As String
    '    Get
    '        Return mBrandingName
    '    End Get
    '    Set(ByVal value As String)
    '        mBrandingName = value
    '    End Set
    'End Property

    'Private mi_CountryCode As Integer
    'Public Overridable Property i_CountryCode() As Integer
    '    Get
    '        Return mi_CountryCode
    '    End Get
    '    Set(ByVal value As Integer)
    '        mi_CountryCode = value
    '    End Set
    'End Property

    Private mCompanyName As String
    Public Overridable Property CompanyName() As String
        Get
            Return mCompanyName
        End Get
        Set(ByVal value As String)
            mCompanyName = value
        End Set
    End Property

    Private mAdditionalData As String
    Public Overridable Property AdditionalData() As String
        Get
            Return mAdditionalData
        End Get
        Set(ByVal value As String)
            mAdditionalData = value
        End Set
    End Property

    Private mGoogleAnalyticCode As String
    Public Overridable Property GoogleAnalyticCode() As String
        Get
            Return mGoogleAnalyticCode
        End Get
        Set(ByVal value As String)
            mGoogleAnalyticCode = value
        End Set
    End Property

    Private mPricePoint As Decimal
    Public Overridable Property PricePoint() As Decimal
        Get
            Return mPricePoint
        End Get
        Set(ByVal value As Decimal)
            mPricePoint = value
        End Set
    End Property

    Private mTrialPricePoint As Decimal
    Public Overridable Property TrialPricePoint() As Decimal
        Get
            Return mTrialPricePoint
        End Get
        Set(ByVal value As Decimal)
            mTrialPricePoint = value
        End Set
    End Property

    Private mCampaignCode As Integer
    Public Overridable Property CampaignCode() As Integer
        Get
            Return mCampaignCode
        End Get
        Set(ByVal value As Integer)
            mCampaignCode = value
        End Set
    End Property

    Private mEmailConversionCID As Integer
    Public Property EmailConversionCID() As Integer
        Get
            Return mEmailConversionCID
        End Get
        Set(ByVal value As Integer)
            mEmailConversionCID = value
        End Set
    End Property

    Private mCulture As String = "en-US" ' Default value
    ''' <summary>
    ''' The domain specific culture to set correctly the current thread's culture. 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Culture() As String
        Get
            Return mCulture
        End Get
        Set(ByVal value As String)
            mCulture = value
        End Set
    End Property

    Private mDomainCache As GMOMCacheEntry
    Private ReadOnly Property DomainCache() As GMOMCacheEntry
        Get
            If mDomainCache Is Nothing Then
                mDomainCache = New GMOMCacheEntry()
                mDomainCache.CacheKey = "Get_Domain_" & Code
                mDomainCache.CacheDuration = 4
            End If
            Return mDomainCache
        End Get
    End Property

#End Region
#Region " Methods "
    Protected Sub Load(ByVal domainCode As Integer)
        If GMOMCache.UseCache AndAlso GMOMCache.IsInCache(DomainCache.CacheKey) Then
            LoadDataFromCache(DomainCache.CacheKey)
        Else
            Dim req As New AirArena.Data.DataRequest("Get_Domain")
            req.Parameters.Add("@DomainCode", domainCode)
            LoadData(req)
        End If
    End Sub

    Protected Sub Load(ByVal domain As String)
        DomainCache.CacheKey = "Get_Domain_" & domain

        If GMOMCache.UseCache AndAlso GMOMCache.IsInCache(DomainCache.CacheKey) Then
            LoadDataFromCache(DomainCache.CacheKey)
        Else
            Dim req As New AirArena.Data.DataRequest("Get_Domain")
            req.Parameters.Add("@DomainName", domain)
            LoadData(req)
        End If
    End Sub

    Protected Sub Load(ByVal value As Integer, ByVal findByNum As FindBy)
        Dim req As New AirArena.Data.DataRequest("Get_Domain")
        req.Parameters.Add("@" & findByNum.ToString, value)
        LoadData(req)
    End Sub

    Private Sub LoadData(ByVal req As AirArena.Data.DataRequest)

        Dim dt As DataTable = DataCon.GetDataTable(req)
        If dt.Rows.Count > 0 Then
            Dim dr As DataRow
            dr = dt.Rows(0)
            Code = CType(dr("i_Code"), Integer)
            Name = CType(dr("vc_Name"), String)
            SupportPhone = CType(dr("vc_SupportPhone"), String)
            Currency = CType(dr("vc_Currency"), String)
            CurrencySymbol = CType(dr("vc_CurrencySymbol"), String)
            'BrandingName = CStr(dr("BrandingName"))
            BrandId = CType(dr("BrandId"), Integer)
            CountryCode = CInt(dr("i_CountryCode"))
            CompanyName = CType(dr("vc_CompanyName"), String)
            AdditionalData = CType(dr("vc_AdditionalData"), String)
            GoogleAnalyticCode = CType(dr("GoogleAnalyticCode"), String)
            PricePoint = CType(dr("PricePoint"), Decimal)
            TrialPricePoint = CType(dr("TrialPricePoint"), Decimal)
            CampaignCode = CType(dr("i_CampaignCode"), Integer)
            mEmailConversionCID = CType(dr("EmailConversionCID"), Integer)
            mSecureSite = CType(dr("SecureSite"), String)
            mCulture = CType(dr("Culture"), String)

            If GMOMCache.UseCache AndAlso Not String.IsNullOrEmpty(DomainCache.CacheKey) Then
                DomainCache.InsertToCache(dr)
            End If

        End If

    End Sub

    Private Sub LoadDataFromCache(ByVal key As String)
        If GMOMCache.IsInCache(key) Then
            Dim dr As DataRow = CType(GMOMCache.RetrieveCacheItem(key), DataRow)
            Code = CType(dr("i_Code"), Integer)
            Name = CType(dr("vc_Name"), String)
            SupportPhone = CType(dr("vc_SupportPhone"), String)
            Currency = CType(dr("vc_Currency"), String)
            CurrencySymbol = CType(dr("vc_CurrencySymbol"), String)
            'BrandingName = CStr(dr("BrandingName"))
            BrandId = CType(dr("BrandId"), Integer)
            CountryCode = CInt(dr("i_CountryCode"))
            CompanyName = CType(dr("vc_CompanyName"), String)
            AdditionalData = CType(dr("vc_AdditionalData"), String)
            GoogleAnalyticCode = CType(dr("GoogleAnalyticCode"), String)
            PricePoint = CType(dr("PricePoint"), Decimal)
            TrialPricePoint = CType(dr("TrialPricePoint"), Decimal)
            CampaignCode = CType(dr("i_CampaignCode"), Integer)
            mEmailConversionCID = CType(dr("EmailConversionCID"), Integer)
            mSecureSite = CType(dr("SecureSite"), String)
            mCulture = CType(dr("Culture"), String)
        End If
    End Sub


    Shared Function ListAllDomains() As List(Of Domain)

        Dim DataCon As New AirArena.Data.DataConnector
        Dim Domains As New List(Of Domain)
        Dim req As New AirArena.Data.DataRequest("List_Domains")
        Dim dt As DataTable = DataCon.GetDataTable(req)
        For Each dr As DataRow In dt.Rows
            Domains.Add(New Domain(CInt(dr("i_Code"))))
        Next dr

        Return Domains

    End Function

    ''' <summary>
    ''' Gets the correct domain name of the requesting domain (Prod or Dev).
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function GetRequestingDomainName() As String
        Dim Domain As String = String.Empty
        If System.Configuration.ConfigurationManager.AppSettings("DebugDomain") = "" Then
            Domain = HttpContext.Current.Request.ServerVariables("SERVER_NAME")
        Else
            Domain = System.Configuration.ConfigurationManager.AppSettings("DebugDomain")
        End If
        Return Domain
    End Function


    'Shared Function IsDomainWithTrackPixelOnPostPaymentPages(ByVal sign As Signup) As Boolean
    '    Dim mIsDomainWithTrackPixelOnPostPaymentPages As Boolean = False
    '    If Not System.Configuration.ConfigurationManager.AppSettings("DomainsWithTrackPixelOnPostPaymentPages") Is Nothing AndAlso sign.Code > 0 Then
    '        Dim DomainsWithTrackPixelOnPostPaymentPages As String()
    '        DomainsWithTrackPixelOnPostPaymentPages = System.Configuration.ConfigurationManager.AppSettings("DomainsWithTrackPixelOnPostPaymentPages").Split(CChar(","))
    '        For Each dom As String In DomainsWithTrackPixelOnPostPaymentPages
    '            If sign.Domain.Country.CountryCode = dom Then
    '                mIsDomainWithTrackPixelOnPostPaymentPages = True
    '            End If
    '        Next
    '    End If
    '    Return mIsDomainWithTrackPixelOnPostPaymentPages
    'End Function

#End Region
End Class
