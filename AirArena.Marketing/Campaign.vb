Public Class Campaign : Inherits CampaignBase
    Public Sub New()
        MyBase.New()
    End Sub
    Public Sub New(ByVal i_Code As Integer)
        MyBase.New()
        Me.Code = i_Code
        If GMOMCache.UseCache Then
            If GMOMCache.IsInCache(CampaignCache.CacheKey) Then
                LoadFromCache()
            Else
                Load(i_Code)
                If Not String.IsNullOrEmpty(CampaignCache.CacheKey) Then
                    GMOMCache.InsertToCacheShared(CampaignCache.CacheKey, Me, 4)
                End If
            End If
        Else
            Load(i_Code)
        End If
    End Sub

    Public Sub New(ByVal i_Code As Integer, ByVal vc_Description As String)
        Me.Code = i_Code
        Me.Description = vc_Description
    End Sub

#Region "Properties"

    Public Enum Products
        Standard = 1
        EBay_PendingURL
        FirstPageListing
        Premium
        AuctionMonster
        GoogleProfitMonster
        eBayProfitMonster
    End Enum

    Private mSignupProductCurrency As AirArena.Accounts.ProductCurrency
    Public Overridable ReadOnly Property SignupProductCurrency() As AirArena.Accounts.ProductCurrency
        Get
            If mSignupProductCurrency Is Nothing Then
                mSignupProductCurrency = New AirArena.Accounts.ProductCurrency(Me.SignupPCID)
            End If
            Return mSignupProductCurrency
        End Get
    End Property

    Private mOngoingProductCurrency As AirArena.Accounts.ProductCurrency
    Public Overridable ReadOnly Property OngoingProductCurrency() As AirArena.Accounts.ProductCurrency
        Get
            If mOngoingProductCurrency Is Nothing Then
                mOngoingProductCurrency = New AirArena.Accounts.ProductCurrency(Me.OngoingPCID)
            End If
            Return mOngoingProductCurrency
        End Get
    End Property

    Private mDomain As Domain
    Public ReadOnly Property Domain() As Domain
        Get
            If mDomain Is Nothing Then
                mDomain = New Domain(CInt(Me.DomainCode))
            End If
            Return mDomain
        End Get
    End Property

    Private mNetwork As Network
    Public ReadOnly Property Network() As Network
        Get
            If mNetwork Is Nothing Then
                mNetwork = New Network(CInt(Me.NetworkCode))
            End If
            Return mNetwork
        End Get
    End Property

    Private mCampaignCache As GMOMCacheEntry
    Private ReadOnly Property CampaignCache() As GMOMCacheEntry
        Get
            If mCampaignCache Is Nothing Then
                mCampaignCache = New GMOMCacheEntry()
                mCampaignCache.CacheKey = "Get_Campaign_" & Code
                mCampaignCache.CacheDuration = 4
            End If
            Return mCampaignCache
        End Get
    End Property

#End Region

#Region "Methods"

    Public ReadOnly Property RehashOptions As List(Of Rehash)
        Get
            Return Rehash.GetRehashOptions(Code)
        End Get
    End Property
    Private _RehashParent As Rehash
    Public ReadOnly Property RehashParent As Rehash
        Get
            If _RehashParent Is Nothing Then
                _RehashParent = Rehash.GetParent(Code)
            End If
            Return _RehashParent
        End Get
    End Property
    Public ReadOnly Property IsRehash As Boolean
        Get
            Return Not RehashParent Is Nothing
        End Get
    End Property

    Private Sub LoadFromCache()
        Dim c As Campaign = CType(GMOMCache.RetrieveCacheItem(CampaignCache.CacheKey), Campaign)
        Me.Code = c.Code
        Me.NetworkCode = c.NetworkCode
        Me.ProductCode = c.ProductCode
        Me.Description = c.Description
        Me.CountryCode = c.CountryCode
        Me.CreateDate = c.CreateDate
        Me.ModifiedDate = c.ModifiedDate
        Me.Status = c.Status
        Me.DomainCode = c.DomainCode
        Me.SignupPCID = c.SignupPCID
        Me.OngoingPCID = c.OngoingPCID
        Me.TrialPeriodInDays = c.TrialPeriodInDays
        Me.TrackingPixelId = c.TrackingPixelId
        Me.ScheduleTypeId = c.ScheduleTypeId
    End Sub

    Public Overrides Sub Save()
        MyBase.Save()
        If GMOMCache.UseCache Then
            CampaignCache.PurgeCacheItem()
        End If
    End Sub

    Shared Function ListAllCampaigns(status As CampaignStatus) As List(Of Campaign)

        Dim DataCon As New AirArena.Data.DataConnector
        Dim Campaigns As New List(Of Campaign)
        Dim req As New AirArena.Data.DataRequest("List_Campaigns")
        req.Parameters.Add("@Active", status)
        Dim dt As DataTable = DataCon.GetDataTable(req)
        For Each dr As DataRow In dt.Rows
            Campaigns.Add(New Campaign(CInt(dr("i_Code"))))
        Next dr

        Return Campaigns

    End Function

    Shared Function ListCampaigns(domainCode As Integer) As List(Of Campaign)

        Dim DataCon As New AirArena.Data.DataConnector
        Dim Campaigns As New List(Of Campaign)
        Dim req As New AirArena.Data.DataRequest("List_Campaigns")
        req.Parameters.Add("@DomainCode", domainCode)
        Dim dt As DataTable = DataCon.GetDataTable(req)
        For Each dr As DataRow In dt.Rows
            Campaigns.Add(New Campaign(CInt(dr("i_Code")), dr("vc_Description").ToString))
        Next dr

        Return Campaigns

    End Function

#End Region
    Public Enum CampaignStatus
        Disabled = 0
        Active = 1
    End Enum
End Class

