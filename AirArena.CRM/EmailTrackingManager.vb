
Public Class EmailTrackingManager

    Public Shared Function TrackEmailAction(ByVal PageRequest As System.Web.HttpRequest, ByVal signupCode As Integer,
                                            ByVal trackingType As TrackingType) As Boolean
        Dim SuccessfulSave As Boolean = False
        Select Case trackingType
            Case EmailTrackingManager.TrackingType.Open

            Case EmailTrackingManager.TrackingType.Click
                If Not String.IsNullOrEmpty(PageRequest.QueryString("etlId")) Then
                    Dim etlId As Integer = 0
                    Integer.TryParse(PageRequest.QueryString("etlId"), etlId)
                    If etlId > 0 Then
                        Dim emailClickTrack As New EmailClickTracker
                        emailClickTrack.EmailTemplateLanguageId = etlId
                        emailClickTrack.SignupCode = signupCode
                        Try
                            emailClickTrack.Save()
                            SuccessfulSave = True
                        Catch ex As Exception
                            SuccessfulSave = False
                        End Try
                    End If
                    If Not PageRequest.QueryString("estId") Is Nothing AndAlso Not PageRequest.QueryString("estvarId") Is Nothing Then
                        Dim emailTestResult As New EmailTest.EmailTestResults.EmailSplitTestResult()
                        With emailTestResult
                            .SingupCode = signupCode
                            .EmailSplitTestId = CInt(PageRequest.QueryString("estId"))
                            .VariationId = CInt(PageRequest.QueryString("estvarId"))
                            .EmailResultType = EmailTest.EmailTestResults.EmailSplitTestResult.EmailSplitTestResultType.EmailClick
                            Try
                                .Save()
                                SuccessfulSave = True
                            Catch ex As Exception
                                SuccessfulSave = False
                            End Try

                        End With
                    End If
                End If
        End Select
        Return SuccessfulSave
    End Function

    Public Enum TrackingType
        Send = 1
        Open = 2
        Click = 3
        Conversion = 4
    End Enum
End Class
