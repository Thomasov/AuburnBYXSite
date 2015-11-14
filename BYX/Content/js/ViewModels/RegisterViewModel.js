function RegisterViewModel() {
    var self = this;
    var getMemberList = $.getJSON("/json/GetMembers").success(function (data) {
        self.memberList(data);
    });
    var getGuardianTypeList = $.getJSON("/json/GetGuardianTypes").success(function (data) {
        self.guardianTypeList(data);
        self.guardianTypeID(data[0].GuardianType_ID);
    });
    self.potentialText = "Thanks for showing interest in BYX! We're really excited to get to know you more and keep up with you throughout the semester, whether you pledge or not. "
                            + "Please contact any of the brothers if you have any questions about the fraternity, pledgeship, or just life in general.";

    self.parentText = "Thanks for entering your information! We're really excited to keep you up to date on what your BYX Brother (or pledge) is doing throughout the semester.";
    self.loading = ko.observable(true);
    self.posting = ko.observable(false);
    self.memberList = ko.observableArray([]);
    self.guardianTypeList = ko.observableArray([]);
    self.type = ko.observable();
    self.fullName = ko.observable();
    self.emailAddress = ko.observable("");
    self.memberID = ko.observable(0);
    self.memberFirstName = ko.observable('this brother');
    self.guardianTypeID = ko.observable(0);

    $.when(getMemberList && getGuardianTypeList).then(function () {
        self.memberID($('#Member_ID option:first').val());
        self.loading(false);
    });
    self.setType = function (newType) {
        self.type(newType);
    };

    self.reset = function () {
        self.type(undefined);
        self.fullName('');
        self.emailAddress('');
        self.memberID($('#Member_ID option:first').val());
        self.memberFirstName = ko.observable('this brother');
        self.guardianTypeID(0);
    }

    self.submit = function () {
        self.posting(true);
        var otherPostData;
        var notType = self.type() == "Parent" ? "Potential" : "Parent";
        self.memberID($('#Member_ID').val());
        if (self.type() == "Parent") {
            otherPostData = {
                GuardianOf: self.memberID(),
                GuardianType_ID: self.guardianTypeID(),
                Archived: false
            };
        }
        else {
            otherPostData = {
                ReferredBy: self.memberID(),
                Archived: false
            };
        }
        var postData = {
            nonmember: {
                FullName: self.fullName(),
                NonMember_EmailAddress: self.emailAddress()
            }
        };
        postData[self.type()] = otherPostData;
        postData[notType] = null;
        $.post('/home/register', postData).success(function (data) {
            if (data.success) {
                alertify.success("Thanks for submitting your information!");
                self.reset();
            }
            else {
                alertify.error("Something went wrong. Please try again later.");
            }
            self.posting(false);
        });
    }
}