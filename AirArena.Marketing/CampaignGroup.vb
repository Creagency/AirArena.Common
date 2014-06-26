Public Class CampaignGroup

#Region "Constructors"

    Public Sub New(ByVal campaignGroupId As Integer, ByVal name As String)
        Me.CampaignGroupId = campaignGroupId
        Me.Name = name
    End Sub

    Public Sub New(ByVal name As String)
        Me.Name = name
    End Sub
#End Region

#Region "Properties"

    Private mCampaignGroupId As Integer
    Public Property CampaignGroupId() As Integer
        Get
            Return mCampaignGroupId
        End Get
        Set(ByVal value As Integer)
            mCampaignGroupId = value
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

#End Region

#Region "Methods"

    Shared Function ListCampaignGroups() As List(Of CampaignGroup)
        Dim DataCon As New AirArena.Data.DataConnector
        Dim CampaignGroups As New List(Of CampaignGroup)
        Dim req As New AirArena.Data.DataRequest("List_CampaignGroups")
        Dim dt As DataTable = DataCon.GetDataTable(req)
        For Each dr As DataRow In dt.Rows
            CampaignGroups.Add(New CampaignGroup(CInt(dr("CampaignGroupId")), dr("Name").ToString()))
        Next dr

        Return CampaignGroups
    End Function

    Public Overridable Sub Save()
        Dim DataCon As New AirArena.Data.DataConnector
        Dim req As New AirArena.Data.DataRequest("AU_CampaignGroup")
        With req.Parameters
            .Add("@Name", mName)
            Dim dt As DataTable = DataCon.GetDataTable(req)
            If dt.Rows.Count > 0 Then
                Me.mCampaignGroupId = CType(dt.Rows(0)("CampaignGroupId"), Integer)
            End If
        End With
    End Sub
#End Region

End Class
