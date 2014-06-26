Imports System.Web

Public Class GMOMCacheEntry

#Region "Constructors"
    Public Sub New()
    End Sub
    Public Sub New(ByVal CacheKey As String, ByVal CacheDuration As Integer)
        Me.CacheKey = CacheKey
        Me.CacheDuration = CacheDuration
    End Sub
#End Region

#Region "Properties"
    Private mCacheKey As String = String.Empty
    Public Property CacheKey() As String
        Get
            Return CStr(mCacheKey)
        End Get
        Set(ByVal value As String)
            mCacheKey = value
        End Set
    End Property

    Private mCacheDuration As Integer = 4
    Public Property CacheDuration() As Integer
        Get
            Return mCacheDuration
        End Get
        Set(ByVal value As Integer)
            mCacheDuration = value
        End Set
    End Property
#End Region

#Region "Methods"
    Public Sub InsertToCache(ByVal obj As Object)
        If mCacheDuration > 0 Then
            HttpContext.Current.Cache.Insert(CacheKey, obj, Nothing, DateTime.Now.AddMinutes(CDbl(CacheDuration)), System.Web.Caching.Cache.NoSlidingExpiration)
        End If
    End Sub

    Public Sub PurgeCacheItem()
        HttpContext.Current.Cache.Remove(CacheKey)
    End Sub

#End Region
End Class

Public Class GMOMCache
#Region "Shared Properties"
    Public Shared ReadOnly Property UseCache() As Boolean
        Get
            Dim mUseCache As Boolean = False
            Boolean.TryParse(System.Configuration.ConfigurationManager.AppSettings("UseCache"), mUseCache)
            Return mUseCache
        End Get
    End Property

    Public Shared Sub InsertToCacheShared(ByVal key As String, ByVal obj As Object, ByVal minutes As Double)
        If minutes > 0 Then
            HttpContext.Current.Cache.Insert(key, obj, Nothing, DateTime.Now.AddMinutes(minutes), System.Web.Caching.Cache.NoSlidingExpiration)
        End If
    End Sub

    Public Shared Function IsInCache(ByVal key As String) As Boolean
        If HttpContext.Current.Cache(key) IsNot Nothing Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Shared Function RetrieveCacheItem(ByVal key As String) As Object
        Return HttpContext.Current.Cache.Get(key)
    End Function
#End Region

End Class
