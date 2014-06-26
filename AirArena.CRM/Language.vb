Public Class Language : Inherits LanguageBase
    Public Sub New()
        MyBase.New()
    End Sub
    Public Sub New(ByVal languageId As Integer)
        MyBase.new(languageId)
    End Sub
    Public Sub New(ByVal code As String)
        MyBase.new()
        Me.Load(code)
    End Sub
    Public Sub New(ByVal dr As DataRow)
        MyBase.new()
        LanguageId = CType(dr("LanguageId"), Integer)
        Name = CType(dr("Name"), String)
        Code = CType(dr("Code"), String)
    End Sub

#Region "Properties"

    Public Shared ReadOnly Property Languages() As List(Of Language)
        Get
            Dim DataCon As New AirArena.Data.DataConnector
            Dim mLanguages As New List(Of Language)
            Dim req As New AirArena.Data.DataRequest("List_Languages")
            Dim dt As DataTable = DataCon.GetDataTable(req)
            For Each dr As DataRow In dt.Rows
                mLanguages.Add(New Language(dr))
            Next dr
            Return mLanguages
        End Get
    End Property

    Public Enum Type
        English = 1
        French
        Spanish
        German
    End Enum

#End Region

#Region "Methods"

    Protected Overloads Sub Load(ByVal code As String)
        Dim req As New AirArena.Data.DataRequest("Get_Language")
        req.Parameters.Add("@Code", code)
        Dim dt As DataTable = DataCon.GetDataTable(req)
        If dt.Rows.Count > 0 Then
            Dim dr As DataRow
            dr = dt.Rows(0)
            Me.LanguageId = CType(dr("LanguageId"), Integer)
            Me.Name = CType(dr("Name"), String)
            Me.Code = CType(dr("Code"), String)
        End If
    End Sub

    Public Shared Function GetLanguageFromBrowser() As String

        Dim haveLanguage As Boolean = False
        Dim LanguageCode As String = Left(System.Threading.Thread.CurrentThread.CurrentCulture.IetfLanguageTag, 2)

        Dim AvailableLanguages() As String = Split(System.Configuration.ConfigurationManager.AppSettings("AvailableLanguages"), ",")
        Dim i As Integer

        For i = 0 To AvailableLanguages.Length - 1
            If LanguageCode = AvailableLanguages(i) Then haveLanguage = True
        Next

        If haveLanguage Then
            Return LanguageCode
        Else
            Return "en"
        End If

    End Function

#End Region

End Class