Public Class Country : Inherits CountryBase
    Public Sub New()
        MyBase.New()
    End Sub
    Public Sub New(ByVal i_Code As Integer)
        MyBase.new()
        Me.Code = i_Code
        If GMOMCache.UseCache AndAlso GMOMCache.IsInCache(CountryCache.CacheKey) Then
            LoadFromCache(CountryCache.CacheKey)
        Else
            Load(i_Code)
            If GMOMCache.UseCache Then
                CountryCache.InsertToCache(Me)
            End If
        End If
    End Sub

#Region "Properties"
    Public ReadOnly Property GMOMDomain() As String
        Get
            Select Case countryCode
                Case "AU"
                    Return "getmeonmedia.com.au"
                Case "FR"
                    Return "getmeonmedia.fr"
                Case "EU"
                    Return "getmeonmedia.eu"
                Case "UK"
                    Return "getmeonmedia.co.uk"
                Case Else
                    Return "getmeonmedia.com"
            End Select
        End Get
    End Property

    Private mCountryCache As GMOMCacheEntry
    Private ReadOnly Property CountryCache() As GMOMCacheEntry
        Get
            If mCountryCache Is Nothing Then
                mCountryCache = New GMOMCacheEntry()
                mCountryCache.CacheKey = "Get_Country_" & Code
                mCountryCache.CacheDuration = 4
            End If
            Return mCountryCache
        End Get
    End Property

#End Region

#Region "Methods"

    Protected Overridable Sub LoadFromCache(ByVal key As String)
        If GMOMCache.IsInCache(key) Then
            Dim co As Country = CType(GMOMCache.RetrieveCacheItem(key), Country)
            Code = co.Code
            vCountryName = co.vCountryName
            CountryCode = co.CountryCode
            GoogleActivationFee = co.GoogleActivationFee
            Description = co.Description
            Status = co.Status
        End If
    End Sub

    Public Overrides Sub Save()
        MyBase.Save()
        If GMOMCache.UseCache Then
            CountryCache.PurgeCacheItem()
        End If
    End Sub

    Shared Function ListAllCountries() As List(Of Country)

        Dim DataCon As New AirArena.Data.DataConnector
        Dim Countries As New List(Of Country)
        Dim req As New AirArena.Data.DataRequest("List_Countries")
        Dim dt As DataTable = DataCon.GetDataTable(req)
        For Each dr As DataRow In dt.Rows
            Countries.Add(New Country(CInt(dr("i_Code"))))
        Next dr

        Return Countries

    End Function

    Shared Function WorldCountriesDropDownList() As DataTable

        Dim DataCon As New AirArena.Data.DataConnector
        Dim req As New AirArena.Data.DataRequest("List_WorldCountries")
        Dim dt As DataTable = DataCon.GetDataTable(req)

        Return dt

    End Function

#End Region

End Class