Imports AirArena.Data

Public Class Rehash
    Private DataCon As DataConnector
    Public Sub New()
        DataCon = New DataConnector()
    End Sub
    Public Sub New(id As Integer)
        Me.New()
        Load(id)
    End Sub
    Protected Friend Sub New(dataRow As DataRow)
        Me.New()
        Load(dataRow)
    End Sub


    Public Sub Save()
        Dim req As New DataRequest("dbo.Rehash_Save")
        If Id > 0 Then
            req.Parameters.Add("@Id", Id)
        End If
        req.Parameters.Add("@SourceCID", SourceCID)
        req.Parameters.Add("@DestinationCID", DestinationCID)
        req.Parameters.Add("@DiscountAmount", DiscountAmount)
        req.Parameters.Add("@IsActive", isActive)
        req.Parameters.Add("@PackageDuration", PackageDuration)
        req.Parameters.Add("@TakeupEventId", TakeupEventId)
        Id = DataCon.ExecuteScalar(Of Integer)(req)
    End Sub
    Public Sub Load(id As Integer)
        Dim req As New DataRequest("dbo.Rehash_Get")
        req.Parameters.Add("@Id", id)
        Dim dt As System.Data.DataTable = DataCon.GetDataTable(req)
        If dt.Rows.Count > 0 Then
            Load(dt.Rows(0))
        End If
    End Sub
    Private Sub Load(dataRow As DataRow)
        Id = CInt(dataRow("Id"))
        SourceCID = CInt(dataRow("SourceCID"))
        DestinationCID = CInt(dataRow("DestinationCID"))
        DiscountAmount = CInt(dataRow("DiscountAmount"))
        isActive = CBool(dataRow("isActive"))
        PackageDuration = CInt(dataRow("PackageDuration"))
        TakeupEventId = CInt(dataRow("TakeupEventId"))
    End Sub
    Private _id As Integer
    Public Property Id As Integer
        Get
            Return _id
        End Get
        Private Set(value As Integer)
            _id = value
        End Set
    End Property
    Public Property SourceCID As Integer
    Private _SourceCampaign As Campaign
    Public ReadOnly Property SourceCampaign As Campaign
        Get
            If SourceCID > 0 Then
                _SourceCampaign = New Campaign(SourceCID)
            End If
            Return _SourceCampaign
        End Get
    End Property
    Public Property DestinationCID As Integer
    Private _DestinationCampaign As Campaign
    Public ReadOnly Property DestinationCampaign As Campaign
        Get
            If DestinationCID > 0 Then
                _DestinationCampaign = New Campaign(DestinationCID)
            End If
            Return _DestinationCampaign
        End Get
    End Property
    Public Property DiscountAmount As Integer
    Public Property isActive As Boolean
    Public Property PackageDuration As Integer
    Public Property TakeupEventId As Integer

    Public Shared Function GetAllRehashOptions() As List(Of Rehash)
        Return GetRehashOptions(-1)
    End Function

    Public Shared Function GetRehashOptions(cID As Integer) As List(Of Rehash)
        Dim rv As New List(Of Rehash)
        Dim dc As New DataConnector()
        Dim req As New DataRequest("dbo.Rehash_List")
        If cID > 0 Then
            req.Parameters.Add("@CID", cID)
        End If
        Dim dt As System.Data.DataTable = dc.GetDataTable(req)
        If dt.Rows.Count > 0 Then
            rv = New List(Of Rehash)
            For Each dr As DataRow In dt.Rows
                rv.Add(New Rehash(dr))
            Next
        End If
        Return rv
    End Function

    Shared Function GetParent(cId As Integer) As Rehash
        Dim rv As Rehash = Nothing
        Dim dc As New DataConnector()
        Dim req As New DataRequest("dbo.Rehash_GetParent")
        req.Parameters.Add("@CID", cId)
        Dim dt As System.Data.DataTable = dc.GetDataTable(req)
        If dt.Rows.Count > 0 Then
            rv = New Rehash(dt.Rows(0))
        End If
        Return rv
    End Function

    Shared Function GetDefault() As Rehash
        Dim rv As New Rehash()
        rv.isActive = True
        rv.PackageDuration = 6
        rv.TakeupEventId = EventType.EventTypeName.Rehash_Customer_Took_Up_6_Month_Offer
        rv.DiscountAmount = 50
        Dim campaigns As List(Of Campaign) = Campaign.ListAllCampaigns(Campaign.CampaignStatus.Active)
        Dim c As Campaign = campaigns.FindLast(AddressOf FindID)
        rv.SourceCID = c.Code
        rv.DestinationCID = c.Code

        Return rv
    End Function
    Private Shared Function FindID(camp As Campaign) As Boolean
        Return True
    End Function


End Class
