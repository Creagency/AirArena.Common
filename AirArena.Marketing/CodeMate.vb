Imports System.Collections.Generic
Imports System.Data
Imports Microsoft.VisualBasic

Public MustInherit Class CampaignBase
    Protected DataCon As AirArena.Data.DataConnector
#Region " Constructors "
    Public Sub New()
        DataCon = New AirArena.Data.DataConnector
    End Sub
    Public Sub New(ByVal code As Integer)
        Me.New()
        Load(code)
    End Sub
#End Region
#Region " Properties "

    Private mSignups As List(Of Signup)
    <Obsolete()> Public Overridable ReadOnly Property Signups() As List(Of Signup)
        Get
            If mSignups Is Nothing Then
                mSignups = New List(Of Signup)
                Dim req As New AirArena.Data.DataRequest("Find_Signups")
                'req.Parameters.add("@SomethingId", SomethingId)
                Dim dt As DataTable = DataCon.GetDataTable(req)
                For Each dr As DataRow In dt.Rows
                    mSignups.Add(New Signup(CInt(dr("SignupsId"))))
                Next dr
            End If
            Return mSignups
        End Get
    End Property

    Private mCode As Integer
    Public Overridable Property Code() As Integer
        Get
            Return mCode
        End Get
        Set(ByVal value As Integer)
            mCode = value
        End Set
    End Property

    Private mNetworkCode As Integer
    Public Overridable Property NetworkCode() As Integer
        Get
            Return mNetworkCode
        End Get
        Set(ByVal value As Integer)
            mNetworkCode = value
        End Set
    End Property


    Private mProductCode As Integer
    Public Overridable Property ProductCode() As Integer
        Get
            Return mProductCode
        End Get
        Set(ByVal value As Integer)
            mProductCode = value
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


    Private mCountryCode As Integer
    Public Overridable Property CountryCode() As Integer
        Get
            Return mCountryCode
        End Get
        Set(ByVal value As Integer)
            mCountryCode = value
        End Set
    End Property

    Private mCountry As Country
    Public Overridable ReadOnly Property Country() As Country
        Get
            If mCountry Is Nothing Then
                mCountry = New Country(mCountryCode)
            End If
            Return mCountry
        End Get
    End Property

    Private mCreateDate As Date
    Public Overridable Property CreateDate() As Date
        Get
            Return mCreateDate
        End Get
        Set(ByVal value As Date)
            mCreateDate = value
        End Set
    End Property


    Private mModifiedDate As Date
    Public Overridable Property ModifiedDate() As Date
        Get
            Return mModifiedDate
        End Get
        Set(ByVal value As Date)
            mModifiedDate = value
        End Set
    End Property


    Private mStatus As Integer
    Public Overridable Property Status() As Integer
        Get
            Return mStatus
        End Get
        Set(ByVal value As Integer)
            mStatus = value
        End Set
    End Property


    Private mDomainCode As Integer
    Public Overridable Property DomainCode() As Integer
        Get
            Return mDomainCode
        End Get
        Set(ByVal value As Integer)
            mDomainCode = value
        End Set
    End Property


    Private mSignupPCID As Integer
    Public Overridable Property SignupPCID() As Integer
        Get
            Return mSignupPCID
        End Get
        Set(ByVal value As Integer)
            mSignupPCID = value
        End Set
    End Property

    Private mOngoingPCID As Integer
    Public Overridable Property OngoingPCID() As Integer
        Get
            Return mOngoingPCID
        End Get
        Set(ByVal value As Integer)
            mOngoingPCID = value
        End Set
    End Property

    Private mTrialPeriodInDays As Integer
    Public Overridable Property TrialPeriodInDays() As Integer
        Get
            Return mTrialPeriodInDays
        End Get
        Set(ByVal value As Integer)
            mTrialPeriodInDays = value
        End Set
    End Property

    Private mTrackingPixelId As Integer
    Public Overridable Property TrackingPixelId() As Integer
        Get
            Return mTrackingPixelId
        End Get
        Set(ByVal value As Integer)
            mTrackingPixelId = value
        End Set
    End Property

    Private mTrackingPixel As TrackingPixel
    Public Overridable ReadOnly Property TrackingPixel() As TrackingPixel
        Get
            If mTrackingPixel Is Nothing Then
                mTrackingPixel = New TrackingPixel(mTrackingPixelId)
            End If
            Return mTrackingPixel
        End Get
    End Property

    Private mScheduleTypeId As Integer
    Public Overridable Property ScheduleTypeId() As Integer
        Get
            Return mScheduleTypeId
        End Get
        Set(ByVal value As Integer)
            mScheduleTypeId = value
        End Set
    End Property
#End Region
#Region " Methods "
    Protected Overridable Sub Load(ByVal code As Integer)
        Dim req As New AirArena.Data.DataRequest("Get_Campaign")
        req.Parameters.Add("@i_Code", code)
        Dim dt As DataTable = DataCon.GetDataTable(req)
        If dt.Rows.Count > 0 Then
            Dim dr As DataRow
            dr = dt.Rows(0)
            mCode = CType(dr("i_Code"), Integer)
            mNetworkCode = CType(dr("i_NetworkCode"), Integer)
            mProductCode = CType(dr("i_ProductCode"), Integer)
            mDescription = CType(dr("vc_Description"), String)
            mCountryCode = CType(dr("i_CountryCode"), Integer)
            mCreateDate = CType(dr("dt_CreateDate"), Date)
            mModifiedDate = CDate(IIf(IsDBNull(dr("dt_ModifiedDate")), Nothing, dr("dt_ModifiedDate")))
            mStatus = CType(dr("i_Status"), Integer)
            mDomainCode = CType(dr("i_DomainCode"), Integer)
            mSignupPCID = CType(dr("i_SignupPCID"), Integer)
            mOngoingPCID = CType(dr("i_OngoingPCID"), Integer)
            mTrialPeriodInDays = CType(dr("TrialPeriodInDays"), Integer)
            mTrackingPixelId = CType(dr("TrackingPixelId"), Integer)
            mScheduleTypeId = CType(dr("ScheduleTypeId"), Integer)
        End If
    End Sub
    Public Overridable Sub Save()
        Dim req As New AirArena.Data.DataRequest("AU_Campaign")
        With req.Parameters
            .Add("@i_Code", mCode)
            .Add("@i_NetworkCode", mNetworkCode)
            .Add("@i_ProductCode", mProductCode)
            .Add("@vc_Description", mDescription)
            .Add("@i_CountryCode", mCountryCode)
            '.Add("@dt_CreateDate", mCreateDate)
            '.Add("@dt_ModifiedDate", mModifiedDate)
            .Add("@i_Status", mStatus)
            .Add("@i_DomainCode", mDomainCode)
            .Add("@i_SignupPCID", mSignupPCID)
            .Add("@i_OngoingPCID", mOngoingPCID)
            .Add("@TrialPeriodInDays", mTrialPeriodInDays)
            .Add("@TrackingPixelId", mTrackingPixelId)
            .Add("@ScheduleTypeId", mScheduleTypeId)
            Dim dt As DataTable = DataCon.GetDataTable(req)
            If dt.Rows.Count > 0 Then
                'TODO: If this object represents a mulit-key table then remove this if/then condition
                Me.mCode = CType(dt.Rows(0)("i_Code"), Integer)
            End If
        End With
    End Sub
#End Region
End Class

Public MustInherit Class CountryBase
    Protected DataCon As AirArena.Data.DataConnector
#Region " Constructors "
    Public Sub New()
        DataCon = New AirArena.Data.DataConnector
    End Sub
    Public Sub New(ByVal i_Code As Integer)
        Me.New()
        Load(i_Code)
    End Sub
#End Region
#Region " Properties "

    Private mDomains As List(Of Domain)
    Public Overridable ReadOnly Property Domains() As List(Of Domain)
        Get
            If mDomains Is Nothing Then
                mDomains = New List(Of Domain)
                Dim req As New AirArena.Data.DataRequest("Find_Domain")
                'req.Parameters.add("@SomethingId", SomethingId)
                Dim dt As DataTable = DataCon.GetDataTable(req)
                For Each dr As DataRow In dt.Rows
                    mDomains.Add(New Domain(CInt(dr("DomainId"))))
                Next dr
            End If
            Return mDomains
        End Get
    End Property

    Private mCode As Integer
    Public Overridable Property Code() As Integer
        Get
            Return mCode
        End Get
        Protected Set(ByVal value As Integer)
            mCode = value
        End Set
    End Property


    Private mCountryName As String
    Public Overridable Property vCountryName() As String
        Get
            Return mCountryName
        End Get
        Set(ByVal value As String)
            mCountryName = value
        End Set
    End Property


    Private mCountryCode As String
    Public Overridable Property CountryCode() As String
        Get
            Return mCountryCode
        End Get
        Set(ByVal value As String)
            mCountryCode = value
        End Set
    End Property


    Private mGoogleActivationFee As String
    Public Overridable Property GoogleActivationFee() As String
        Get
            Return mGoogleActivationFee
        End Get
        Set(ByVal value As String)
            mGoogleActivationFee = value
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


    Private mStatus As Integer
    Public Overridable Property Status() As Integer
        Get
            Return mStatus
        End Get
        Set(ByVal value As Integer)
            mStatus = value
        End Set
    End Property

#End Region
#Region " Methods "
    Protected Overridable Sub Load(ByVal i_Code As Integer)
        Dim req As New AirArena.Data.DataRequest("Get_Country")
        req.Parameters.Add("@i_Code", i_Code)
        Dim dt As DataTable = DataCon.GetDataTable(req)
        If dt.Rows.Count > 0 Then
            Dim dr As DataRow
            dr = dt.Rows(0)
            mCode = CType(dr("i_Code"), Integer)
            mCountryName = CType(dr("vc_CountryName"), String)
            mCountryCode = CType(dr("vc_CountryCode"), String)
            mGoogleActivationFee = CType(dr("GoogleActivationFee"), String)
            mDescription = CType(dr("vc_Description"), String)
            mStatus = CType(dr("i_Status"), Integer)
        End If
    End Sub
    Public Overridable Sub Save()
        Dim req As New AirArena.Data.DataRequest("AU_Country")
        With req.Parameters
            .Add("@i_Code", mCode)
            .Add("@vc_CountryName", mCountryName)
            .Add("@vc_CountryCode", mCountryCode)
            .Add("@GoogleActivationFee", mGoogleActivationFee)
            .Add("@vc_Description", mDescription)
            .Add("@i_Status", mStatus)
            Dim dt As DataTable = DataCon.GetDataTable(req)
            If dt.Rows.Count > 0 Then
                'TODO: If this object represents a mulit-key table then remove this if/then condition
                Me.mCode = CType(dt.Rows(0)("i_Code"), Integer)
            End If
        End With
    End Sub
#End Region
End Class

Public MustInherit Class ProductBase
    Protected DataCon As AirArena.Data.DataConnector
#Region " Constructors "
    Public Sub New()
        DataCon = New AirArena.Data.DataConnector
    End Sub
    Public Sub New(ByVal productId As Integer)
        Me.New()
        Load(productId)
    End Sub
#End Region
#Region " Properties "

    Shared ReadOnly Property Products() As List(Of Product)
        Get
            Dim DataCon As New AirArena.Data.DataConnector
            Dim mProducts As New List(Of Product)
            Dim req As New AirArena.Data.DataRequest("List_Products")
            req.Parameters.Add("@Active", 1)
            Dim dt As DataTable = DataCon.GetDataTable(req)
            For Each dr As DataRow In dt.Rows
                mProducts.Add(New Product(CInt(dr("ProductId"))))
            Next dr
            Return mProducts
        End Get
    End Property

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

    Private mProductId As Integer
    Public Overridable Property ProductId() As Integer
        Get
            Return mProductId
        End Get
        Protected Set(ByVal value As Integer)
            mProductId = value
        End Set
    End Property


    Private mName As String
    Public Overridable Property Name() As String
        Get
            Return mName
        End Get
        Set(ByVal value As String)
            mName = value
        End Set
    End Property


    Private mActive As Integer
    Public Overridable Property Active() As Integer
        Get
            Return mActive
        End Get
        Set(ByVal value As Integer)
            mActive = value
        End Set
    End Property

#End Region
#Region " Methods "
    Protected Overridable Sub Load(ByVal productId As Integer)
        Dim req As New AirArena.Data.DataRequest("Get_Product")
        req.Parameters.Add("@productId", productId)
        Dim dt As DataTable = DataCon.GetDataTable(req)
        If dt.Rows.Count > 0 Then
            Dim dr As DataRow
            dr = dt.Rows(0)
            mProductId = CType(dr("ProductId"), Integer)
            mName = CType(dr("Name"), String)
            mActive = CType(dr("Active"), Integer)
        End If
    End Sub
    Public Overridable Sub Save()
        Dim req As New AirArena.Data.DataRequest("AU_Product")
        With req.Parameters
            .Add("@ProductId", mProductId)
            .Add("@Name", mName)
            .Add("@Active", mActive)
            Dim dt As DataTable = DataCon.GetDataTable(req)
            If dt.Rows.Count > 0 Then
                'TODO: If this object represents a mulit-key table then remove this if/then condition
                Me.mProductId = CType(dt.Rows(0)("ProductId"), Integer)
            End If
        End With
    End Sub
#End Region
End Class

Public MustInherit Class NetworkBase
    Protected DataCon As AirArena.Data.DataConnector
#Region " Constructors "
    Public Sub New()
        DataCon = New AirArena.Data.DataConnector
    End Sub
    Public Sub New(ByVal networkCode As Integer)
        Me.New()
        Load(networkCode)
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

    Private mNetworkId As Integer
    Public Overridable Property NetworkId() As Integer
        Get
            Return mNetworkId
        End Get
        Protected Set(ByVal value As Integer)
            mNetworkId = value
        End Set
    End Property

    Private mNetworkName As String
    Public Overridable Property NetworkName() As String
        Get
            Return mNetworkName
        End Get
        Set(ByVal value As String)
            mNetworkName = value
        End Set
    End Property

    Private mDTCreation As Date
    Public Overridable Property DTCreation() As Date
        Get
            Return mDTCreation
        End Get
        Set(ByVal value As Date)
            mDTCreation = value
        End Set
    End Property

    Private mStatus As Integer
    Public Overridable Property Status() As Integer
        Get
            Return mStatus
        End Get
        Set(ByVal value As Integer)
            mStatus = value
        End Set
    End Property

#End Region
#Region " Methods "
    Protected Overridable Sub Load(ByVal i_Code As Integer)
        Dim req As New AirArena.Data.DataRequest("Get_Network")
        req.Parameters.Add("@i_Code", i_Code)
        Dim dt As DataTable = DataCon.GetDataTable(req)
        If dt.Rows.Count > 0 Then
            Dim dr As DataRow
            dr = dt.Rows(0)
            mNetworkId = CType(dr("i_Code"), Integer)
            mNetworkName = CType(dr("vc_NetworkName"), String)
            mDTCreation = CType(dr("dt_CreateDate"), Date)
            mStatus = CType(dr("i_Status"), Integer)
        End If
    End Sub
    Public Overridable Sub Save()
        Dim req As New AirArena.Data.DataRequest("AU_Network")
        With req.Parameters
            .Add("@i_Code", mNetworkId)
            .Add("@vc_NetworkName", mNetworkName)
            .Add("@dt_CreateDate", mDTCreation)
            .Add("@i_Status", mStatus)
            Dim dt As DataTable = DataCon.GetDataTable(req)
            If dt.Rows.Count > 0 Then
                'TODO: If this object represents a mulit-key table then remove this if/then condition
                Me.mNetworkId = CType(dt.Rows(0)("i_Code"), Integer)
            End If
        End With
    End Sub
#End Region
End Class

Public MustInherit Class EventTypeBase
    Protected DataCon As AirArena.Data.DataConnector
#Region " Constructors "
    Public Sub New()
        DataCon = New AirArena.Data.DataConnector
    End Sub
    Public Sub New(ByVal eventTypeId As Integer)
        DataCon = New AirArena.Data.DataConnector
        Load(eventTypeId)
    End Sub
#End Region
#Region " Properties "

    Private mEventTypeId As Integer
    Public Overridable Property EventTypeId() As Integer
        Get
            Return mEventTypeId
        End Get
        Protected Set(ByVal value As Integer)
            mEventTypeId = value
        End Set
    End Property


    Private mName As String
    Public Overridable Property Name() As String
        Get
            Return mName
        End Get
        Set(ByVal value As String)
            mName = value
        End Set
    End Property

#End Region
#Region " Methods "
    Protected Overridable Sub Load(ByVal eventTypeId As Integer)
        Dim req As New AirArena.Data.DataRequest("Get_EventType")
        req.Parameters.Add("@EventTypeId", eventTypeId)
        Dim dt As DataTable = DataCon.GetDataTable(req)
        If dt.Rows.Count > 0 Then
            Dim dr As DataRow
            dr = dt.Rows(0)
            mEventTypeId = CType(dr("EventTypeId"), Integer)
            mName = CType(dr("Name"), String)
        End If
    End Sub
    Public Overridable Sub Save()
        Dim req As New AirArena.Data.DataRequest("AU_EventType")
        With req.Parameters
            .Add("@EventTypeId", mEventTypeId)
            .Add("@Name", mName)

            Dim dt As DataTable = DataCon.GetDataTable(req)
            If dt.Rows.Count > 0 Then
                Me.mEventTypeId = CType(dt.Rows(0)("EventTypeId"), Integer)
            End If
        End With
    End Sub
#End Region
End Class