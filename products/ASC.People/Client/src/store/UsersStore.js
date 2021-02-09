import { action, makeObservable, observable } from "mobx";
import { api, constants, store } from "asc-web-common";
import { isMobileOnly } from "react-device-detect";

const { Filter } = api;
const { EmployeeStatus, EmployeeActivationStatus } = constants;
const { authStore } = store;
class UsersStore {
  users = null;

  constructor(peopleStore) {
    this.peopleStore = peopleStore;
    makeObservable(this, {
      users: observable,
      getUsersList: action,
    });
  }

  getUsersList = async (filter) => {
    let filterData = filter && filter.clone();

    if (!filterData) {
      filterData = Filter.getDefault();
      filterData.employeeStatus = EmployeeStatus.Active;
    }

    const res = await api.people.getUserList(filterData);
    filterData.total = res.total;

    this.peopleStore.filterStore.setFilterParams(filterData);
    this.peopleStore.selectedGroupStore.setSelectedGroup(filterData.group);

    this.users = res.items;
    console.log("user: ", this.users.length);
  };

  removeUser = async (userId, filter) => {
    await api.people.deleteUsers(userId);
    this.getUsersList(filter);
  };

  updateUserStatus = async (status, userIds, isRefetchPeople = false) => {
    await api.people.updateUserStatus(status, userIds);
    const filter = this.peopleStore.filterStore.filter;
    isRefetchPeople && this.getUsersList(filter);
  };

  updateUserType = async (type, userIds) => {
    const res = await api.people.updateUserType(type, userIds);
    res.forEach((user) => this.users.push(user));
  };

  updateProfileInUsers = async (updatedProfile) => {
    if (!this.users) {
      return this.getUsersList();
    }
    if (!updatedProfile) {
      //TODO
    }
  };

  getStatusType = (user) => {
    if (
      user.status === EmployeeStatus.Active &&
      user.activationStatus === EmployeeActivationStatus.Activated
    ) {
      return "normal";
    } else if (
      user.status === EmployeeStatus.Active &&
      user.activationStatus === EmployeeActivationStatus.Pending
    ) {
      return "pending";
    } else if (user.status === EmployeeStatus.Disabled) {
      return "disabled";
    } else {
      return "unknown";
    }
  };

  getUserRole = (user) => {
    if (user.isOwner) return "owner";
    else if (user.isAdmin) return "admin";
    //TODO: Need refactoring
    else if (user.isVisitor) return "guest";
    else return "user";
  };

  getUserContextOptions = (
    isMySelf,
    isOwner,
    statusType,
    status,
    hasMobileNumber
  ) => {
    const options = [];

    switch (statusType) {
      case "normal":
      case "unknown":
        options.push("send-email");

        if (hasMobileNumber && isMobileOnly) {
          options.push("send-message");
        }

        options.push("separator");
        options.push("edit");
        options.push("change-password");
        options.push("change-email");

        if (isMySelf) {
          if (!isOwner) {
            options.push("delete-self-profile");
          }
        } else {
          options.push("disable");
        }

        break;
      case "disabled":
        options.push("enable");
        //TODO: Need implementation
        /*options.push("reassign-data");
        options.push("delete-personal-data");*/
        options.push("delete-profile");
        break;
      case "pending":
        options.push("edit");
        options.push("invite-again");

        if (isMySelf) {
          options.push("delete-profile");
        } else {
          if (status === EmployeeStatus.Active) {
            options.push("disable");
          } else {
            options.push("enable");
          }
        }
        break;
      default:
        break;
    }

    return options;
  };

  composePeopleList = () => {
    return this.users.map((user) => {
      const {
        id,
        displayName,
        avatar,
        email,
        isOwner,
        isAdmin: isAdministrator,
        isVisitor,
        mobilePhone,
        userName,
        activationStatus,
        status,
        groups,
      } = user;
      const statusType = this.getStatusType(user);
      const role = this.getUserRole(user);
      const isMySelf = user.userName === authStore.userStore.user.userName;
      const isViewerAdmin = authStore.userStore.user.isAdmin;

      const options = this.getUserContextOptions(
        isMySelf,
        isOwner,
        statusType,
        status,
        !!mobilePhone
      );

      return {
        id,
        checked: isViewerAdmin ? false : undefined,
        status,
        activationStatus,
        statusType,
        role,
        isOwner,
        isAdmin: isAdministrator,
        isVisitor,
        displayName,
        avatar,
        email,
        userName,
        mobilePhone,
        options,
        groups,
      };
    });
  };
}

export default UsersStore;
