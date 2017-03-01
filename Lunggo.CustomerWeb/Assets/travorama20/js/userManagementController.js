app.controller('userManagementController', [
    '$http', '$scope', '$log', function ($http, $scope, $log) {

        var hash = (location.hash);
        // variables

        //General Variables
        $scope.pageLoaded = true;
        $scope.email = 'Test';
        $scope.trial = 0;
        $scope.loading = false;
        $scope.enableEdit = false;
        $scope.currentSection = '';
        $scope.profileForm = {
            active: false
        };
        $scope.userData = {
            email: '',
            role: ''
        }
        $scope.roleData = {
            email: '',
            role: ''
        }

        //Get User
        $scope.User = {
            GetUser: function () {
                if ($scope.trial > 3) {
                    $scope.trial = 0;
                }
                var authAccess = getAuthAccess();
                if (authAccess == 2) {
                    $http({
                        method: 'GET',
                        url: GetUserConfig.Url,
                        async: false,
                        headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') }
                    }).then(function (returnData) {
                        if (returnData.status == "200") {
                            $log.debug('Success getting All Users');
                            $scope.roles = returnData.data.roles;
                            $scope.users = returnData.data.users;
                        }
                        else {
                            $log.debug('There is an error');
                            $log.debug('Error : ' + returnData.data.error);
                            $log.debug(returnData);
                        }
                    }).catch(function (returnData) {
                        $scope.trial++;
                        if (refreshAuthAccess() && $scope.trial < 4) //refresh cookie
                        {
                            $scope.User.GetUser();
                        }
                        else {
                            $log.debug('Failed to Get Profile');
                        }
                    });
                }
                else {
                    $log.debug('Not Authorized');
                }
            }
        }

        //Update Role

        $scope.setUpdateRole = function() {
            $scope.enableEdit = true;
        }

        $scope.cancelUpdate= function()
        {
            $scope.enableEdit = false;
        }


        $scope.updateRole = function (email) {
            $scope.roleUpdated = false;
            var authAccess = getAuthAccess();
            if (authAccess == 2) {
                //authorized
                $http({
                    url: UpdateRoleConfig.Url,
                    method: 'POST',
                    data: {
                        userName: email,
                        role: $scope.roleData.role
                    },
                    headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') }
                }).then(function (returnData) {
                    //$log.debug(returnData);
                    if (returnData.data.status == '200') {
                        $log.debug('Success updating Profile');
                        $scope.roleUpdated = true;

                    }
                    else {
                        $log.debug(returnData.data.error);
                        $log.debug(returnData);
                        $scope.roleUpdated = false;
                    }
                }).catch(function (returnData) {
                    $scope.trial++;
                    if (refreshAuthAccess() && $scope.trial < 4) //refresh cookie
                    {
                        $scope.updateRole(email, role);
                    }
                    else {
                        $log.debug('Failed Update Role');
                        $log.debug(returnData);
                        $scope.roleUpdated = false;
                    }
                });
            }
            else { //if not authorized
                $scope.roleUpdated = false;
            }
        }

        $scope.closePopUp = function() {
            $scope.userAdded = false;
            $scope.roleUpdated = false;
            $scope.userDeleted = false;
            window.location.reload();
        }

        //Add User
        $scope.addUser = function () {
            $scope.userAdded = false;
            var authAccess = getAuthAccess();
            if (authAccess == 2) {
                //authorized
                $http({
                    url: AddUserConfig.Url,
                    method: 'POST',
                    data: {
                        email: $scope.userData.email,
                        role: $scope.userData.role
                    },
                    headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') }
                }).then(function (returnData) {
                    if (returnData.data.status == '200') {
                        $log.debug('Success Add User');
                        $scope.userAdded = true;
                    }
                    else {
                        $log.debug(returnData.data.error);
                        $log.debug(returnData);
                        $scope.userAdded = false;
                    }
                }).catch(function (returnData) {
                    $scope.trial++;
                    if (refreshAuthAccess() && $scope.trial < 4) //refresh cookie
                    {
                        $scope.addUser();
                    }
                    else {
                        $log.debug('Failed Add User');
                        $log.debug(returnData);
                        $scope.userAdded = false;
                    }
                });
            }
            else { //if not authorized
                $scope.userAdded = false;
            }
        }

        //Delete User
        $scope.deleteUser = function (email) {
            $scope.userDeleted = false;
            var authAccess = getAuthAccess();
            if (authAccess == 2) {
                //authorized
                $http({
                    url: DeleteUserConfig.Url,
                    method: 'POST',
                    data: {
                        email: email
                    },
                    headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') }
                }).then(function (returnData) {
                    if (returnData.data.status == '200') {
                        $log.debug('Success Add User');
                        $scope.userDeleted = true;
                    }
                    else {
                        $log.debug(returnData.data.error);
                        $log.debug(returnData);
                        $scope.userDeleted = false;
                    }
                }).catch(function (returnData) {
                    $scope.trial++;
                    if (refreshAuthAccess() && $scope.trial < 4) //refresh cookie
                    {
                        $scope.deleteUser();
                    }
                    else {
                        $log.debug('Failed Add User');
                        $log.debug(returnData);
                        $scope.userDeleted = false;
                    }
                });
            }
            else { //if not authorized
                $scope.userDeleted = false;
            }
        }

        //Executing Get User
        $scope.User.GetUser();

        $scope.changeSection = function (name) {
            $scope.currentSection = name;
        }

        if (hash == '#order') {
            $scope.changeSection('order');
        } else {
            $scope.changeSection('profile');
        }

    }
]);