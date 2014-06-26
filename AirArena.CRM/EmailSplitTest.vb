Imports System.Collections.Generic
Imports System.Xml
Imports System.Text
Imports System.IO

Namespace EmailTest
    Public Class EmailSplitTest

        Private _emailSplitTestId As Integer
        Private _emailTestId As Integer
        Private _emailSplitTestTypeId As Int16
        Private _emailSplitTestVariations As List(Of EmailSplitTestContent) = Nothing
        Private _randomSelectedTestVariation As EmailSplitTestContent = Nothing
        'Private TestName As String
        'Private IsActive As Boolean
        'Private EmailTemplateLanguageToSplit As Integer
        'Private dt_Created As DateTime
        'Private dt_Started As DateTime
        'Private dt_ended As DateTime



        Protected DataCon As AirArena.Data.DataConnector
#Region " Constructors "
        Public Sub New()
            DataCon = New AirArena.Data.DataConnector
        End Sub


        Public Sub New(ByVal emailSplitTestId As Integer)
            Me.new()
            Load(emailSplitTestId)
        End Sub

        Public Sub New(ByVal emailTestId As Integer, ByVal emailSplitTestTypeId As EmailSplitTestTypes)
            Me.New()
            Load(emailTestId, emailSplitTestTypeId)
        End Sub
        'Public Sub New(ByVal emailTestId As Integer, ByVal emailSplitTestTypeId As EmailSplitTestTypes)
        '    Me.new()
        '    Load(emailTestId, emailSplitTestTypeId)
        'End Sub
#End Region



        Public Property EmailSplitTestId() As Integer
            Get
                Return _emailSplitTestId
            End Get
            Set(ByVal value As Integer)
                _emailSplitTestId = value
            End Set
        End Property

        Public Property EmailTestId() As Integer
            Get
                Return _emailTestId
            End Get
            Set(ByVal value As Integer)
                _emailTestId = value
            End Set
        End Property


        Public Property EmailSplitTestTypeId() As Int16
            Get
                Return _emailSplitTestTypeId
            End Get
            Set(ByVal value As Int16)
                _emailSplitTestTypeId = value
            End Set
        End Property

        ''' <summary>
        ''' Get all test variations. by default only live variation
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property EmailSplitTestVariations() As List(Of EmailSplitTestContent)
            Get
                If _emailSplitTestVariations Is Nothing Then
                    GetAllTestVariations(True)
                End If
                Return _emailSplitTestVariations
            End Get
        End Property

        ''' <summary>
        ''' Get all test variation. use overload to specify if live only or all
        ''' </summary>
        ''' <param name="GetOnlyLive"></param>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property EmailSplitTestVariations(ByVal GetOnlyLive As Boolean) As List(Of EmailSplitTestContent)
            Get
                GetAllTestVariations(GetOnlyLive)
                Return _emailSplitTestVariations
            End Get
        End Property

        Public Property RandomSelectedTestVariation() As EmailSplitTestContent
            Get
                Return _randomSelectedTestVariation
            End Get
            Set(ByVal value As EmailSplitTestContent)
                _randomSelectedTestVariation = value
            End Set
        End Property

        Public Enum EmailSplitTestTypes
            From = 1
            Subject
            Body
            LandingPageURL
            CompleteEmail
        End Enum

#Region " Methods "

        Protected Sub Load(ByVal emailSplitTestId As Integer)
            Dim req As New AirArena.Data.DataRequest("Get_EmailSplitTest")
            req.Parameters.Add("@EmailSplitTestId", emailSplitTestId)
            Dim dt As DataTable = DataCon.GetDataTable(req)
            If dt.Rows.Count > 0 Then
                'est.EmailSplitTestId, est.EmailTestId, est.EmailSplitTestTypeId
                Dim row As DataRow = dt.Rows(0)
                _emailSplitTestId = CType(row("EmailSplitTestId"), Integer)
                _emailTestId = CType(row("EmailTestId"), Integer)
                _emailSplitTestTypeId = CType(row("EmailSplitTestTypeId"), Int16)
            End If

        End Sub


        Protected Sub Load(ByVal emailTestId As Integer, ByVal emailSplitTestTypeId As EmailSplitTestTypes)
            Dim req As New AirArena.Data.DataRequest("Get_EmailSplitTest")
            req.Parameters.Add("@EmailTestId", emailTestId)
            'req.Parameters.Add("@emailSplitTestTypeId", emailSplitTestTypeId)
            Dim dt As DataTable = DataCon.GetDataTable(req)
            If dt.Rows.Count > 0 Then
                'est.EmailSplitTestId, est.EmailTestId, est.EmailSplitTestTypeId
                Dim row As DataRow = dt.Rows(0)
                _emailSplitTestId = CType(row("EmailSplitTestId"), Integer)
                _emailTestId = CType(row("EmailTestId"), Integer)
                _emailSplitTestTypeId = CType(row("EmailSplitTestTypeId"), Int16)
            End If

        End Sub

        Protected Sub GetAllTestVariations()
            GetAllTestVariations(True)
        End Sub

        Protected Sub GetAllTestVariations(ByVal GetOnlyLive As Boolean)
            Dim req As New AirArena.Data.DataRequest("List_EmailSplitTestVariations")
            req.Parameters.Add("@EmailSplitTestId", EmailSplitTestId)
            Dim dt As DataTable = DataCon.GetDataTable(req)
            _emailSplitTestVariations = New List(Of EmailSplitTestContent)
            If dt.Rows.Count > 0 Then
                'est.EmailSplitTestId, est.EmailTestId, est.EmailSplitTestTypeId
                For Each row As DataRow In dt.Rows
                    Dim Variation As New EmailSplitTestContent()
                    'SELECT estc.VariationId,estc.EmailSplitTestId,estc.ContentTypeId,
                    'estc.SplitTestContent, estc.VariationSplitPercentage, estc.IsVariationActive
                    Variation.VariationId = CType(row("VariationId"), Integer)
                    Variation.EmailSplitTestId = CType(row("EmailSplitTestId"), Integer)
                    Variation.ContentType = CType(row("ContentTypeId"), EmailSplitTestContent.SplitTestContentType)
                    Variation.Content = CStr(row("SplitTestContent"))
                    Variation.SplitPercentage = CType(row("VariationSplitPercentage"), Integer)
                    Variation.VariationIsActive = CType(row("IsVariationActive"), Boolean)
                    If GetOnlyLive And Variation.VariationIsActive Then
                        _emailSplitTestVariations.Add(Variation)
                    ElseIf Not GetOnlyLive Then
                        _emailSplitTestVariations.Add(Variation)
                    End If

                Next
            End If
        End Sub

        ''' <summary>
        ''' Gets a random variation - checks if the test as auto percent or not and useses the needed function
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetRandomTestVariation() As EmailSplitTestContent
            Dim AllVariation As List(Of EmailSplitTestContent) = EmailSplitTestVariations(True)
            Dim SelectedRandomVariation As New EmailSplitTestContent()
            Dim testHasPercenteages As Boolean = True
            For Each var As EmailSplitTestContent In AllVariation
                If var.SplitPercentage = -1 Then
                    testHasPercenteages = False
                End If
            Next
            If testHasPercenteages Then
                'SortListWithPercent(_RandomLinkslst) ' Fix any percent issues.
                '_RandomLinkslst.Sort(New DictionaryLinkRotator())
                'RandomLink = GetRandomLinkWithPrecentage(_RandomLinkslst)
                SelectedRandomVariation = GetRandomVariationWithPrecentage(AllVariation)
            Else
                SelectedRandomVariation = GetRandomVariation(AllVariation)
            End If
            Return SelectedRandomVariation
        End Function

        Public Function GetRandomTestVariation(ByVal usePercentage As Boolean) As EmailSplitTestContent
            Dim AllVariation As List(Of EmailSplitTestContent) = EmailSplitTestVariations()
            Dim SelectedRandomVariation As New EmailSplitTestContent()
            If usePercentage Then
                'SortListWithPercent(_RandomLinkslst) ' Fix any percent issues.
                '_RandomLinkslst.Sort(New DictionaryLinkRotator())
                'RandomLink = GetRandomLinkWithPrecentage(_RandomLinkslst)
            Else
                SelectedRandomVariation = GetRandomVariation(AllVariation)
            End If
            Return SelectedRandomVariation
        End Function

        Private Function GetRandomVariation(ByVal AllVariation As List(Of EmailSplitTestContent)) As EmailSplitTestContent
            If AllVariation.Count > 0 Then
                Dim NumberOfVariation As Integer = AllVariation.Count
                Dim Rand As New Random()
                Dim RandomLink As Integer = Rand.Next(0, NumberOfVariation)
                Return AllVariation(RandomLink)
            Else
                Return Nothing
            End If
        End Function

        Private Function GetRandomVariationWithPrecentage(ByVal AllVariation As List(Of EmailSplitTestContent)) As EmailSplitTestContent
            Dim RandomVariationBase As New Random()
            Dim totalPercentageSum As Integer = 0
            For Each variation As EmailSplitTestContent In AllVariation
                totalPercentageSum += variation.SplitPercentage
            Next
            If AllVariation.Count > 0 Then
                Dim RandomVariation As Integer = RandomVariationBase.Next(1, totalPercentageSum + 1)
                Dim SubSum As Integer = 0
                Dim i As Integer = 0
                Dim RandomSelectedVariation As New EmailSplitTestContent()
                While (SubSum < RandomVariation) And (i < AllVariation.Count)
                    SubSum += AllVariation(i).SplitPercentage
                    RandomSelectedVariation = AllVariation(i)
                    i += 1
                End While

                Return RandomSelectedVariation

                ' Second option.
                'If i = 0 Then
                '    i = 1
                'End If
                'Return Links(i - 1)
            Else
                Return Nothing
            End If
        End Function
       
        Public Shared Function SaveEmailSplitTest(ByVal emailTestId As Integer, ByVal emailSplitTestTypeId As EmailSplitTestTypes) As EmailSplitTest
            Dim DataCon As New AirArena.Data.DataConnector()
            Dim emailSplitTest As EmailSplitTest = Nothing
            Dim req As New AirArena.Data.DataRequest("AU_EmailSplitTest")
            req.Parameters.Add("@emailTestId", emailTestId)
            req.Parameters.Add("@emailSplitTestTypeId", emailSplitTestTypeId)
            Dim dt As DataTable = DataCon.GetDataTable(req)
            If dt.Rows.Count > 0 Then
                emailSplitTest = New EmailSplitTest(CType(dt.Rows(0)("EmailSplitTestId"), Integer))
            End If
            Return emailSplitTest
        End Function

        Public Shared Function SaveEtl2SplitTest(ByVal emailSplitTestId As Integer, ByVal EmailTemplateLanguageId As Integer) As Boolean
            Dim SavedEtl2SplitTest As Boolean = False
            Dim DataCon As New AirArena.Data.DataConnector()
            Dim req As New AirArena.Data.DataRequest("AU_Etl2SplitTest")
            req.Parameters.Add("@emailSplitTestId", emailSplitTestId)
            req.Parameters.Add("@EmailTemplateLanguageId", EmailTemplateLanguageId)
            Dim dt As DataTable = DataCon.GetDataTable(req)
            If dt.Rows.Count > 0 AndAlso CInt(dt.Rows(0)("records")) > 0 Then
                SavedEtl2SplitTest = True
            End If
            Return SavedEtl2SplitTest
        End Function


#End Region


        Public Shared Function ListEmailSplitTestTypes() As List(Of EmailSplitTestType)
            Dim req As New AirArena.Data.DataRequest("List_EmailSplitTestType")
            Dim DataCon As New AirArena.Data.DataConnector()
            Dim dt As DataTable = DataCon.GetDataTable(req)
            Dim splitTestTypes As New List(Of EmailSplitTestType)
            If dt.Rows.Count > 0 Then
                For Each row As DataRow In dt.Rows
                    Dim testType As New EmailSplitTestType()
                    testType.SplitTestTypeId = CType(row("SplitTestTypeId"), Int16)
                    testType.SplitTestTypeName = CStr(row("Name"))
                    splitTestTypes.Add(testType)
                Next
            End If
            Return splitTestTypes
        End Function


        Public Shared Function IsEmailLanaguageTemplateSplitTest(ByVal EmailTemplateLanguageId As Integer) As Boolean
            Dim IsSplitTest As Boolean = False
            Dim DataCon As New AirArena.Data.DataConnector()
            Dim req As New AirArena.Data.DataRequest("Get_Etl2SplitTest")
            req.Parameters.Add("@EmailTemplateLanguageId", EmailTemplateLanguageId)
            Dim dt As DataTable = DataCon.GetDataTable(req)
            If dt.Rows.Count > 0 AndAlso CInt(dt.Rows(0)("EmailSplitTestId")) > 0 Then
                IsSplitTest = CType(dt.Rows(0)("IsActive"), Boolean)
            End If
            Return IsSplitTest
        End Function

        Public Shared Function GetSplitTestFromEmailLanaguageTemplate(ByVal EmailTemplateLanguageId As Integer) As EmailSplitTest
            Dim est As New EmailSplitTest()
            Dim DataCon As New AirArena.Data.DataConnector()
            Dim req As New AirArena.Data.DataRequest("Get_Etl2SplitTest")
            req.Parameters.Add("@EmailTemplateLanguageId", EmailTemplateLanguageId)
            Dim dt As DataTable = DataCon.GetDataTable(req)
            If dt.Rows.Count > 0 AndAlso CInt(dt.Rows(0)("EmailSplitTestId")) > 0 Then
                est = New EmailSplitTest(CInt(dt.Rows(0)("EmailSplitTestId")))
            End If
            Return est
        End Function



        'Public Sub LoadSplitTestContent(ByRef etl As EmailTemplateLanguage, ByVal emailSplitTest As EmailSplitTest)
        '    'Dim emailSplitTest As New EmailSplitTest()
        '    'emailSplitTest = GMOMBL.EmailTest.EmailSplitTest.GetSplitTestFromEmailLanaguageTemplate(etl.EmailTemplateLanguageId)

        '    Dim testVariation As EmailSplitTestContent = emailSplitTest.GetRandomTestVariation(False)
        '    testVariation = emailSplitTest.GetRandomTestVariation()
        '    Select Case emailSplitTest.EmailSplitTestTypeId
        '        Case CShort(GMOMBL.EmailTest.EmailSplitTest.EmailSplitTestTypes.Subject)
        '            etl.Subject = testVariation.Content
        '        Case CShort(GMOMBL.EmailTest.EmailSplitTest.EmailSplitTestTypes.From)
        '            etl.MailFrom = testVariation.Content
        '    End Select
        'End Sub

        Public Function LoadSplitTestContent(ByRef etl As EmailTemplateLanguage, ByVal emailSplitTest As EmailSplitTest) As EmailSplitTestContent
            'Dim emailSplitTest As New EmailSplitTest()
            'emailSplitTest = GMOMBL.EmailTest.EmailSplitTest.GetSplitTestFromEmailLanaguageTemplate(etl.EmailTemplateLanguageId)

            Dim testVariation As EmailSplitTestContent = emailSplitTest.GetRandomTestVariation(False)
            testVariation = emailSplitTest.GetRandomTestVariation()
            If Not testVariation Is Nothing Then
                Select Case emailSplitTest.EmailSplitTestTypeId
                    Case CShort(EmailSplitTestTypes.Subject)
                        etl.Subject = testVariation.Content
                    Case CShort(EmailSplitTestTypes.From)
                        etl.MailFrom = testVariation.Content
                    Case CShort(EmailSplitTestTypes.Body)
                        etl.Body = testVariation.Content
                    Case CShort(EmailSplitTestTypes.CompleteEmail)
                        etl.Body = testVariation.ParseCompleteEmailXmlToEtl(testVariation.Content).Body
                        etl.Subject = testVariation.ParseCompleteEmailXmlToEtl(testVariation.Content).Subject
                        etl.MailFrom = testVariation.ParseCompleteEmailXmlToEtl(testVariation.Content).MailFrom
                End Select
            End If

            Return testVariation
        End Function
        Public Structure EmailSplitTestType
            Public Property SplitTestTypeId() As Int16
            Public Property SplitTestTypeName() As String
        End Structure


    End Class


    Public Class EmailSplitTestContent
       
        Private _emailSplitTest As EmailSplitTest
        Public ReadOnly Property EmailSplitTest() As EmailSplitTest
            Get
                If _emailSplitTest Is Nothing Then
                    _emailSplitTest = New EmailSplitTest(EmailSplitTestId)
                End If
                Return _emailSplitTest
            End Get
        End Property

        Public Property VariationId() As Integer

        Public Property EmailSplitTestId() As Integer

        Public Property ContentType() As SplitTestContentType = SplitTestContentType.Variation
        Public Property Content() As String
        Public Property SplitPercentage() As Integer = -1 ' -1 means Auto
        Public Property VariationIsActive() As Boolean = True

        Public Enum SplitTestContentType
            Control = 1
            Variation
        End Enum

        Protected DataCon As AirArena.Data.DataConnector
#Region " Constructors "
        Public Sub New()
            DataCon = New AirArena.Data.DataConnector
        End Sub


        'Public Sub New(ByVal EmailSplitTestId As Integer)
        '    Me.New()
        'End Sub
#End Region


        'Protected Sub Load(ByVal EmailSplitTestId As Integer)
        '    Dim req As New AirArena.Data.DataRequest("Get_EmailSplitTestContent")
        '    req.Parameters.Add("@EmailSplitTestId", EmailSplitTestId)
        '    Dim dt As DataTable = DataCon.GetDataTable(req)
        '    If dt.Rows.Count > 0 Then
        '        'est.EmailSplitTestId, est.EmailTestId, est.EmailSplitTestTypeId
        '        Dim row As DataRow = dt.Rows(0)
        '        _EmailSplitTestId = CType(row("EmailSplitTestId"), Integer)
        '        _emailTestId = CType(row("EmailTestId"), Integer)
        '        _emailSplitTestTypeId = CType(row("EmailSplitTestTypeId"), Int16)
        '    End If
        'End Sub

        Public Sub Save()

            If (EmailSplitTest.EmailSplitTestId > 0 AndAlso Not String.IsNullOrEmpty(Content)) Then
                Dim req As New AirArena.Data.DataRequest("AU_EmailSplitTestContent")
                req.Parameters.Add("@EmailSplitTestId", EmailSplitTest.EmailSplitTestId)
                If Not VariationId > 0 Then
                    req.Parameters.Add("@ContentType", ContentType)
                End If
                req.Parameters.Add("@Content", Content)
                req.Parameters.Add("@SplitPercentage", SplitPercentage)
                req.Parameters.Add("@VariationIsActive", VariationIsActive)
                If VariationId > 0 Then
                    req.Parameters.Add("@VariationId", VariationId)
                End If

                Dim dt As DataTable = DataCon.GetDataTable(req)
                If dt.Rows.Count > 0 Then
                    If Not VariationId > 0 Then
                        VariationId = CType(dt.Rows(0)("VariationId"), Integer)
                    End If
                End If
            End If

        End Sub

        ''' <summary>
        ''' Parse complete email from XML file to email template language object
        ''' </summary>
        ''' <param name="testContentXml"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ParseCompleteEmailXmlToEtl(ByVal testContentXml As String) As EmailTemplateLanguage
            Dim etl As EmailTemplateLanguage = Nothing
            Dim ContentAsString As New StringBuilder()
            Using reader As XmlReader = XmlReader.Create(New StringReader(testContentXml))
                If reader.Read() Then
                    etl = New EmailTemplateLanguage()
                    While reader.Read()
                        If reader.NodeType = XmlNodeType.Element Then
                            If reader.Name.ToLower() = "from" Then
                                etl.MailFrom = reader.ReadElementContentAsString
                            End If
                            If reader.Name.ToLower() = "subject" Then
                                etl.Subject = reader.ReadElementContentAsString
                            End If
                            If reader.Name.ToLower() = "body" Then
                                etl.Body = reader.ReadElementContentAsString
                            End If
                        End If
                    End While
                End If

            End Using
            Return etl
        End Function

        ''' <summary>
        ''' Parse complete email from XML file to string
        ''' </summary>
        ''' <param name="testContentXml"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ParseCompleteEmailXmlToString(ByVal testContentXml As String) As String
            Dim ContentAsString As New StringBuilder()
            Using reader As XmlReader = XmlReader.Create(New StringReader(testContentXml))
                While reader.Read()
                    If reader.NodeType = XmlNodeType.Element Then
                        If reader.Name.ToLower() = "from" Then
                            ContentAsString.AppendLine("<b>From</b>:" + reader.ReadElementContentAsString + "<br/>")
                        End If
                        If reader.Name.ToLower() = "subject" Then
                            ContentAsString.AppendLine("<b>Subject:</b>" + reader.ReadElementContentAsString + "<br/>")
                        End If
                        If reader.Name.ToLower() = "body" Then
                            ContentAsString.AppendLine("<b>body:</b>" + reader.ReadElementContentAsString)
                        End If
                    End If
                End While
            End Using
            Return ContentAsString.ToString()
        End Function

        ''' <summary>
        ''' Create xml file to represent complete email variation
        ''' </summary>
        ''' <param name="etl">Email template language object</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CreateCompleteEmailXml(ByVal etl As EmailTemplateLanguage) As String

            Dim xmlFile As New StringBuilder()
            Try
                Dim settings As XmlWriterSettings = New XmlWriterSettings()
                settings.Indent = True
                settings.OmitXmlDeclaration = False
                Using writer As XmlWriter = XmlWriter.Create(xmlFile, settings)
                    writer.WriteStartDocument()
                    writer.WriteStartElement("EmailSplitTestContent")
                    writer.WriteStartElement("From")
                    writer.WriteCData(etl.MailFrom)
                    writer.WriteEndElement()
                    writer.WriteStartElement("Subject")
                    writer.WriteCData(etl.Subject)
                    writer.WriteEndElement()
                    writer.WriteStartElement("Body")
                    writer.WriteCData(etl.Body)
                    writer.WriteEndElement()
                    writer.WriteEndElement()
                    writer.Flush()
                End Using
            Catch ex As Exception

            End Try
            Return xmlFile.ToString()

        End Function




    End Class


End Namespace

