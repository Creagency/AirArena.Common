
Public Class EmailCampaign : Inherits EmailCampaignBase

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal emailCampaignId As Integer)
        MyBase.New(emailCampaignId)
    End Sub

    Public Sub New(ByVal dr As DataRow)
        MyBase.New(dr)
    End Sub

    Public Overloads Sub Save()
        MyBase.Save()
    End Sub

    Public Shared Function SaveSentEmailCampaign(ByVal emailCampaignId As Integer, ByVal lastSendSignupCode As Integer, _
                                            ByVal recipientsNumber As Integer) As Boolean
        Dim SuccessfullSave As Boolean = False
        Dim DataCon As New AirArena.Data.DataConnector()
        Dim req As New AirArena.Data.DataRequest("AU_SentEmailCampaign")
        req.Parameters.Add("@EmailCampaignId", emailCampaignId)
        req.Parameters.Add("@LastSendSignupCode", lastSendSignupCode)
        req.Parameters.Add("@RecipientsNumber", recipientsNumber)
        Dim dt As DataTable = DataCon.GetDataTable(req)
        If dt.Rows.Count > 0 Then
            If CType(dt.Rows(0)("i_Code"), Integer) > 0 Then
                SuccessfullSave = True
            End If
        End If
        Return SuccessfullSave
    End Function

    Public Shared Function GetAllEmailCampaigns() As List(Of EmailCampaign)
        Return _getAllEmailCampaigns()
    End Function

    'Public Shared Function GetAllScheduledCampaigns(ByVal CampaignStatus As EmailCampaignStatusName) As List(Of EmailCampaign)
    Public Shared Function GetAllScheduledCampaigns() As List(Of EmailCampaign)
        Dim DataCon As New AirArena.Data.DataConnector
        Dim AllCampaigns As New List(Of EmailCampaign)
        Dim req As New AirArena.Data.DataRequest("Admin_List_ScheduledEmailCampaign")
        req.Parameters.Add("@CampaignStatusId", CInt(EmailCampaign.EmailCampaignStatusName.Queued))
        Dim dt As DataTable = DataCon.GetDataTable(req)
        For Each dr As DataRow In dt.Rows
            Dim emailCampaign As New EmailCampaign(dr)
            AllCampaigns.Add(emailCampaign)
        Next dr
        Return AllCampaigns

    End Function

    ''' <summary>
    ''' Gets the list of signups for that campaign and populates the last send signup code
    ''' </summary>
    ''' <param name="emailCampaign"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetEmailCampaignSignupList(ByVal emailCampaign As EmailCampaign) As List(Of Marketing.Signup)
        Dim signups As New List(Of Marketing.Signup)
        Dim DataCon As New AirArena.Data.DataConnector()
        Dim req As New AirArena.Data.DataRequest("Admin_GET_Signups_EmailCampaign")
        req.Parameters.Add("@NumToSend", emailCampaign.RecipientsNumber)
        req.Parameters.Add("@EmailCampaignId", emailCampaign.EmailCampaignId)
        req.Parameters.Add("@StatusId", emailCampaign.SignupStatusId)
        req.Parameters.Add("@CountryIDs", emailCampaign.CountryIDs)
        req.Parameters.Add("@LanguageId", emailCampaign.LanguageId)
        req.Parameters.Add("@LastSendSignupCode", 0)
        '@NumToSend INT = 0
        ',@EmailTemplateLanguageId int
        ',@StatusId int = 1 -- By default we send only to unpaid	
        ',@LastSendSignupCode INT OUTPUT

        Dim ds As DataSet = DataCon.GetDataSet(req)
        If ds.Tables.Count = 2 Then
            For Each row As DataRow In ds.Tables(0).Rows
                signups.Add(New Marketing.Signup(row))
            Next
            Me.LastSignupCode = CInt(ds.Tables(1).Rows(0)(0)) ' Get last signup in list. would use as start point for next
            'send
        End If
        Return signups
    End Function

    'Public Shared Sub TestReflection()
    '    Dim AssemblyName As String = "GMOMBL.dll"
    '    Dim objName As String = "Signup"
    '    Dim FullObjName As String = "GMOMBL.Signup"

    '    Dim assem As System.Reflection.Assembly = System.Reflection.Assembly.GetExecutingAssembly()

    '    Dim assemName As System.Reflection.AssemblyName = assem.GetName()

    '    Dim t As Type = assem.GetType(assemName.Name + "." + objName)


    '    Dim obj3 As Object = Activator.CreateInstance(t)

    '    Dim signObj As Object = Activator.CreateInstance("GMOMBL", "GMOMBL.Signup")
    '    Dim obj2 As Object = System.Runtime.CompilerServices.RuntimeHelpers.GetObjectValue(signObj)
    'End Sub

    Public Enum EmailCampaignStatusName
        Draft = 1
        Queued = 2
        InProgress = 3
        Sent = 4
        Errored = 5
        ReScheduled = 6
    End Enum

End Class
