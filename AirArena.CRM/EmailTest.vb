Imports System.Data

Namespace EmailTest
    Public Class EmailTest

#Region " Properties"


        Private _emailTestId As Integer
        Private _emailTestTypeId As Int16
        Private _testName As String
        Private _isActive As Boolean
        Private _emailTemplateLanguageToSplit As Integer
        'Private _dt_Created As DateTime

        Private _dt_Created As Nullable(Of DateTime)
        'Private _dt_Started As DateTime
        Private _dt_Started As Nullable(Of DateTime)
        Private _dt_ended As DateTime

        Public Property EmailTestId() As Integer
            Get
                Return _emailTestId
            End Get
            Set(ByVal value As Integer)
                _emailTestId = value
            End Set
        End Property


        Public Property EmailTestTypeId() As Int16
            Get
                Return _emailTestTypeId
            End Get
            Set(ByVal value As Int16)
                _emailTestTypeId = value
            End Set
        End Property

        Public Property TestName() As String
            Get
                Return _testName
            End Get
            Set(ByVal value As String)
                _testName = value
            End Set
        End Property

        Public Property EmailTemplateLanguageToSplit() As Integer
            Get
                Return _emailTemplateLanguageToSplit
            End Get
            Set(ByVal value As Integer)
                _emailTemplateLanguageToSplit = value
            End Set
        End Property

        Public Property IsActive() As Boolean
            Get
                Return _isActive
            End Get
            Set(ByVal value As Boolean)
                _isActive = value
            End Set
        End Property


        'Private _dt_Created As DateTime
        'Private _dt_Started As DateTime
        'Private _dt_ended As DateTime

        'Public Property dt_Created() As DateTime
        '    Get
        '        Return _dt_Created
        '    End Get
        '    Set(ByVal value As DateTime)
        '        _dt_Created = value
        '    End Set
        'End Property


        Public Property dt_Created() As Nullable(Of DateTime)
            Get
                Return _dt_Created.GetValueOrDefault()
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                _dt_Created = value
            End Set
        End Property

        Public Property dt_Started() As Nullable(Of DateTime)
            Get
                If _dt_Started.HasValue Then
                    Return _dt_Started
                Else
                    Return _dt_Started.GetValueOrDefault()
                End If
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                'If value.HasValue Then
                '    _dt_Started = value
                'End If
                _dt_Started = value
            End Set
        End Property

        Public Property dt_Ended() As DateTime
            Get
                Return _dt_ended
            End Get
            Set(ByVal value As DateTime)
                _dt_ended = value
            End Set
        End Property

#End Region



        Protected DataCon As AirArena.Data.DataConnector
#Region " Constructors "
        Public Sub New()
            DataCon = New AirArena.Data.DataConnector
        End Sub

        Public Sub New(ByVal code As Integer)
            Me.new()
            Load(code)
        End Sub

        'Public Sub New(ByVal value As String, ByVal fieldName As String)
        '    Me.new()
        '    Load(value, fieldName)
        'End Sub

        Public Enum EmailTestType
            SplitTest = 1
            MultiVariant
        End Enum

#End Region


#Region " Methods "
        Protected Sub Load(ByVal emailTestId As Integer)
            Dim req As New AirArena.Data.DataRequest("Get_EmailTest")
            req.Parameters.Add("@EmailTestId", emailTestId)
            Dim dt As DataTable = DataCon.GetDataTable(req)
            If dt.Rows.Count > 0 Then
                Dim row As DataRow
                row = dt.Rows(0)
                _emailTestId = CType(row("emailTestId"), Integer)
                _emailTestTypeId = CType(row("TestTypeId"), Int16)
                _testName = CType(row("TestName"), String)
                _isActive = CType(row("IsActive"), Boolean)
                _emailTemplateLanguageToSplit = CType(row("EmailTemplateLanguageId"), Integer)


                '_dt_Created = CType(row("dt_Created"), DateTime)
                Me.dt_Created = CType(row("dt_Created"), Nullable(Of DateTime))


                '!! Leave at the moment the usage of Nullables for dates*******

                '_dt_Started = CType(row("dt_Started"), Nullable(Of DateTime))
                '  Me.dt_Started = CType(row("dt_Started"), Nullable(Of DateTime))
                'Me.dt_Started = CType(row("dt_Started"), DateTime?)

                'this row works!
                'Me.dt_Started = CType(IIf(row.IsNull("dt_Started"), Nothing, row("dt_Started")), DateTime)

                Me.dt_Started = CType(IIf(row.IsNull("dt_Started"), Nothing, row("dt_Started")), Nullable(Of DateTime))

                '_dt_Started = CType(IIf(row.IsNull("dt_Started"), row("dt_Started"), CType(row("dt_Started"), DateTime)), Date?)

                Me.dt_Ended = CType(IIf(row.IsNull("dt_Ended"), Nothing, row("dt_Ended")), DateTime)
                'emailTestId, TestTypeId, TestName, IsActive
                ',EmailTemplateLanguageId, dt_Created , dt_Started, dt_Ended

                If Me.dt_Created Is Nothing Then

                End If

                If Me.dt_Started Is Nothing Then

                End If
            End If
        End Sub

        Public Shared Function CreateEmailTest(ByVal testType As EmailTestType, ByVal testName As String, ByVal emailTemplateLanguageId As Integer) As EmailTest
            Dim emailTest As EmailTest = Nothing
            Dim DataCon As New AirArena.Data.DataConnector()
            Dim req As New AirArena.Data.DataRequest("AU_EmailTest")
            req.Parameters.Add("@TestTypeId", CInt(testType))
            req.Parameters.Add("@TestName", testName)
            req.Parameters.Add("@EmailTemplateLanguageId", emailTemplateLanguageId)
            Dim dt As DataTable = DataCon.GetDataTable(req)
            If dt.Rows.Count > 0 Then
                emailTest = New EmailTest(CType(dt.Rows(0)("EmailTestId"), Integer))
            End If
            Return emailTest
        End Function

        Public Overridable Sub Save()
            Dim DataCon As New AirArena.Data.DataConnector
            Dim req As New AirArena.Data.DataRequest("AU_EmailTest")
            With req.Parameters
                req.Parameters.Add("@EmailTestId", _emailTestId)
                req.Parameters.Add("@IsActive", _isActive)
                req.Parameters.Add("@TestTypeId", _emailTestTypeId)
                req.Parameters.Add("@TestName", _testName)
                req.Parameters.Add("@EmailTemplateLanguageId", _emailTemplateLanguageToSplit)
                Dim dt As DataTable = DataCon.GetDataTable(req)
                If dt.Rows.Count > 0 Then
                    Me._emailTestId = CType(dt.Rows(0)("EmailTestId"), Integer)
                End If
            End With
        End Sub
#End Region

#Region " Shared Methods "
        Public Shared Function GetAllEmailTest() As List(Of EmailTest)
            Dim req As New AirArena.Data.DataRequest("List_EmailTest")
            Dim DataCon As New AirArena.Data.DataConnector()
            Dim dt As DataTable = DataCon.GetDataTable(req)
            Dim AllEmailTests As New List(Of EmailTest)
            If dt.Rows.Count > 0 Then
                For Each row As DataRow In dt.Rows
                    Dim ThisEmailTest As New EmailTest()
                    With ThisEmailTest
                        .EmailTestId = CType(row("emailTestId"), Integer)
                        .EmailTestTypeId = CType(row("TestTypeId"), Int16)
                        .TestName = CType(row("TestName"), String)
                        .IsActive = CType(row("IsActive"), Boolean)
                        .EmailTemplateLanguageToSplit = CType(row("EmailTemplateLanguageId"), Integer)


                        '_dt_Created = CType(row("dt_Created"), DateTime)
                        .dt_Created = CType(row("dt_Created"), Nullable(Of DateTime))


                        '!! Leave at the moment the usage of Nullables for dates*******

                        '_dt_Started = CType(row("dt_Started"), Nullable(Of DateTime))
                        '  Me.dt_Started = CType(row("dt_Started"), Nullable(Of DateTime))
                        'Me.dt_Started = CType(row("dt_Started"), DateTime?)

                        'this row works!
                        'Me.dt_Started = CType(IIf(row.IsNull("dt_Started"), Nothing, row("dt_Started")), DateTime)

                        .dt_Started = CType(IIf(row.IsNull("dt_Started"), Nothing, row("dt_Started")), Nullable(Of DateTime))

                        '_dt_Started = CType(IIf(row.IsNull("dt_Started"), row("dt_Started"), CType(row("dt_Started"), DateTime)), Date?)

                        .dt_Ended = CType(IIf(row.IsNull("dt_Ended"), Nothing, row("dt_Ended")), DateTime)

                    End With
                    AllEmailTests.Add(ThisEmailTest)

                Next

            End If
            Return AllEmailTests
        End Function
#End Region

    End Class
End Namespace
