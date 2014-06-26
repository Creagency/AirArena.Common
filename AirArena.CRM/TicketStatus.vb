Imports System.Collections.Generic
Imports System.Data

Public Class TicketStatus : Inherits TicketStatusBase
    Public Sub New()
        MyBase.New()
    End Sub
    Public Sub New(ByVal ticketStatusId As Integer)
        MyBase.new(ticketStatusId)
    End Sub
#Region " Properties "

    Public Enum Status 'Removed status Resolved, as TicketRecipient makes it redundant
        Closed
        [New]
        TicketTransfered
        RequireCustomerResponse
        CustomerResponded
        Resolved
    End Enum

    Private mTicketStatuses As List(Of TicketStatus)
    Public ReadOnly Property TicketStatuses(Optional ByVal mode As String = Nothing) As List(Of TicketStatus)
        Get
            If mTicketStatuses Is Nothing Then
                mTicketStatuses = New List(Of TicketStatus)
                Dim req As New AirArena.Data.DataRequest("List_TicketStatuses")
                Dim dt As DataTable = DataCon.GetDataTable(req)

                Dim allowed As Boolean = True
                Dim i As Integer = 1

                For Each dr As DataRow In dt.Rows
                    If mode = "AdminEditting" Then
                        Select Case i
                            Case 1 'New
                                allowed = False
                            Case 2 'Require Customer Response
                                allowed = True
                            Case 3 'Customer Responded
                                allowed = False
                            Case 4 'Follow up
                                allowed = True
                            Case 5 'Resolved
                                allowed = True
                            Case 6 'Closed
                                allowed = False
                        End Select

                        If allowed = True Then mTicketStatuses.Add(New TicketStatus(CInt(dr("TicketStatusId"))))
                        i = i + 1
                        allowed = True
                    Else
                        mTicketStatuses.Add(New TicketStatus(CInt(dr("TicketStatusId"))))
                    End If
                Next dr
            End If
            Return mTicketStatuses
        End Get
    End Property

#End Region
#Region "Methods"

#End Region
End Class

