Public Class EmailClickTracker : Inherits EmailTrackingBase

#Region " Constructors "
    Public Sub New()
        DataCon = New AirArena.Data.DataConnector
    End Sub
    Public Sub New(ByVal emailTemplateLanguageId As Integer)
        Me.New()
        '  Load(emailTemplateLanguageId)
    End Sub

#End Region

#Region "Private Fields"
    Private mClicks As Integer = 0
#End Region

#Region "Properties"

    Public Overridable Property Clicks() As Integer
        Get
            Return mClicks
        End Get
        Set(ByVal value As Integer)
            mClicks = value
        End Set
    End Property


#End Region

#Region "Methods"


    Public Overrides Sub Save()
        Dim req As New AirArena.Data.DataRequest("AU_EmailClickTrackerSave")
        With req.Parameters
            .Add("@i_SignupCode", Me.SignupCode)
            .Add("@EmailTemplateLanguageId", Me.EmailTemplateLanguageId)
            Dim dt As DataTable = DataCon.GetDataTable(req)
            If dt.Rows.Count > 0 Then
                Me.Code = CType(dt.Rows(0)("i_code"), Integer)
            End If
        End With
    End Sub






#End Region



End Class
