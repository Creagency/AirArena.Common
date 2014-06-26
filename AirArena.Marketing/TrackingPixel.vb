Public Class TrackingPixel : Inherits TrackingPixelBase
    Public Sub New()
        MyBase.New()
    End Sub
    Public Sub New(ByVal trackingPixelId As Integer)
        MyBase.new(trackingPixelId)
    End Sub

#Region "Methods"

    Shared Function ListAllTrackingPixels() As List(Of TrackingPixel)

        Dim DataCon As New AirArena.Data.DataConnector
        Dim TrackingPixels As New List(Of TrackingPixel)
        Dim req As New AirArena.Data.DataRequest("List_TrackingPixels")
        Dim dt As DataTable = DataCon.GetDataTable(req)
        For Each dr As DataRow In dt.Rows
            TrackingPixels.Add(New TrackingPixel(CInt(dr("TrackingPixelId"))))
        Next dr

        Return TrackingPixels

    End Function

#End Region

End Class

Public MustInherit Class TrackingPixelBase
    Protected DataCon As AirArena.Data.DataConnector
#Region " Constructors "
    Public Sub New()
        DataCon = New AirArena.Data.DataConnector
    End Sub
    Public Sub New(ByVal trackingPixelId As Integer)
        Me.New()
        Load(trackingPixelId)
    End Sub
#End Region
#Region " Properties "

    Private mCampaigns As List(Of Campaign)
    Public Overridable ReadOnly Property Campaigns() As List(Of Campaign)
        Get
            If mCampaigns Is Nothing Then
                mCampaigns = New List(Of Campaign)
                Dim req As New AirArena.Data.DataRequest("Find_Campaign")
                'req.Parameters.add("@SomethingId", SomethingId)
                Dim dt As DataTable = DataCon.GetDataTable(req)
                For Each dr As DataRow In dt.Rows
                    mCampaigns.Add(New Campaign(CInt(dr("CampaignId"))))
                Next dr
            End If
            Return mCampaigns
        End Get
    End Property

    Private mTrackingPixelId As Integer
    Public Overridable Property TrackingPixelId() As Integer
        Get
            Return mTrackingPixelId
        End Get
        Protected Set(ByVal value As Integer)
            mTrackingPixelId = value
        End Set
    End Property

    Private mDescription As String
    Public Overridable Property Description() As String
        Get
            Return mDescription
        End Get
        Set(ByVal value As String)
            mDescription = value
        End Set
    End Property

    Private mTrackingPixel As String
    Public Overridable Property TrackingPixel() As String
        Get
            Return mTrackingPixel
        End Get
        Set(ByVal value As String)
            mTrackingPixel = value
        End Set
    End Property

    Private mDisplayPosition As String
    Public Overridable Property DisplayPosition() As String
        Get
            Return mDisplayPosition
        End Get
        Set(ByVal value As String)
            mDisplayPosition = value
        End Set
    End Property

#End Region
#Region " Methods "
    Protected Overridable Sub Load(ByVal trackingPixelId As Integer)
        Dim req As New AirArena.Data.DataRequest("Get_TrackingPixel")
        req.Parameters.Add("@trackingPixelId", trackingPixelId)
        Dim dt As DataTable = DataCon.GetDataTable(req)
        If dt.Rows.Count > 0 Then
            Dim dr As DataRow
            dr = dt.Rows(0)
            mTrackingPixelId = CType(dr("TrackingPixelId"), Integer)
            mDescription = CType(dr("Description"), String)
            mTrackingPixel = CType(dr("TrackingPixel"), String)
            mDisplayPosition = CType(dr("DisplayPosition"), String)
        End If
    End Sub
    Public Overridable Sub Save()
        Dim req As New AirArena.Data.DataRequest("AU_TrackingPixel")
        With req.Parameters
            .Add("@TrackingPixelId", mTrackingPixelId)
            .Add("@Description", mDescription)
            .Add("@TrackingPixel", mTrackingPixel)
            .Add("@DisplayPosition", mDisplayPosition)
            Dim dt As DataTable = DataCon.GetDataTable(req)
            If dt.Rows.Count > 0 Then
                'TODO: If this object represents a mulit-key table then remove this if/then condition
                Me.mTrackingPixelId = CType(dt.Rows(0)("TrackingPixelId"), Integer)
            End If
        End With
    End Sub
#End Region
End Class