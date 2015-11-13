function RegisterViewModel() {
    var self = this;
    var getMemberList = $.getJSON("/json/GetMembers").success(function (data) {
        self.memberList(data);
        self.memberID(data[0].Member_ID);
    });
    var getGuardianTypeList = $.getJSON("/json/GetGuardianTypes").success(function (data) {
        self.guardianTypeList(data);
        self.guardianTypeID(data[0].GuardianType_ID);
    });
    self.potentialText = "Thanks for showing interest in BYX! We're really excited to get to know you more and keep up with you throughout the semester, whether you pledge or not. "
                            + "Please contact any of the brothers if you have any questions about the fraternity, pledgeship, or just life in general.";

    self.parentText = "Thanks for entering your information! We're really excited to keep you up to date on what your BYX Brother (or pledge) is doing throughout the semester.";
    self.loading = ko.observable(true);
    self.memberList = ko.observableArray([]);
    self.guardianTypeList = ko.observableArray([]);
    self.type = ko.observable();
    self.fullName = ko.observable();
    self.emailAddress = ko.observable("");
    self.memberID = ko.observable(0);
    self.memberFirstName = ko.observable('this brother');
    self.guardianTypeID = ko.observable(0);

    $.when(getMemberList && getGuardianTypeList).then(function () {
        self.loading(false);
    });
    self.setType = function (newType) {
        self.type(newType);
    };

    self.submit = function () {
        var person = {
            FullName: self.fullName(),
            NonMember_EmailAddress: self.emailAddress()
        }
        console.log(person);
    }
}