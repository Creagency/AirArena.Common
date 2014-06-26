Imports System.Data
Imports System.Web

Public Class Branding
    Protected DataCon As AirArena.Data.DataConnector
#Region " Constructors "
    Public Sub New()
        DataCon = New AirArena.Data.DataConnector
    End Sub
    Public Sub New(ByVal brandId As Integer)
        Me.New()
        Me.BrandId = brandId
        If GMOMCache.UseCache AndAlso GMOMCache.IsInCache(CacheKey) Then
            LoadFromCache(CacheKey)
        Else
            Load(brandId)
        End If
    End Sub

#End Region
#Region " Properties "

    Private mBrandId As Integer
    Public Property BrandId() As Integer
        Get
            Return mBrandId
        End Get
        Protected Set(ByVal value As Integer)
            mBrandId = value
        End Set
    End Property

    Private mName As String
    Public Property Name() As String
        Get
            Return mName
        End Get
        Set(ByVal value As String)
            mName = value
        End Set
    End Property

    Private mDomainDirectory As String
    Public Overridable Property DomainDirectory() As String
        Get
            Return mDomainDirectory
        End Get
        Set(ByVal value As String)
            mDomainDirectory = value
        End Set
    End Property

    Private mArticle As String
    Public Overridable Property Article() As String
        Get
            Return mArticle
        End Get
        Set(ByVal value As String)
            mArticle = value
        End Set
    End Property

    Private mStatus As Integer
    Public Property Status() As Integer
        Get
            Return mStatus
        End Get
        Set(ByVal value As Integer)
            mStatus = value
        End Set
    End Property

    Private ReadOnly Property CacheKey() As String
        Get
            Return "Get_Branding_" & BrandId
        End Get
    End Property

#End Region
#Region " Methods "
    Protected Sub Load(ByVal brandId As Integer)
        Dim req As New AirArena.Data.DataRequest("Get_Branding")
        req.Parameters.Add("@BrandId", brandId)
        Dim dt As DataTable = DataCon.GetDataTable(req)
        If dt.Rows.Count > 0 Then
            Dim dr As DataRow
            dr = dt.Rows(0)
            PopulateValues(dr)

            If GMOMCache.UseCache Then
                GMOMCache.InsertToCacheShared(CacheKey, dr, 4)
            End If

        End If
    End Sub

    Protected Sub LoadFromCache(ByVal key As String)
        If GMOMCache.IsInCache(key) Then
            Dim dr As DataRow = CType(GMOMCache.RetrieveCacheItem(key), DataRow)
            PopulateValues(dr)
        End If
    End Sub

    Protected Sub PopulateValues(ByVal dr As DataRow)
        Me.BrandId = CType(dr("BrandId"), Integer)
        Me.Name = CType(dr("Name"), String)
        Me.DomainDirectory = Trim(CType(dr("DomainDirectory"), String))
        Me.Article = CType(dr("Article"), String)
        Me.Status = CType(dr("Status"), Integer)
    End Sub

#End Region
End Class