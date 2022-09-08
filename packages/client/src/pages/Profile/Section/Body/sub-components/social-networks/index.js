import React, { useEffect } from "react";
import styled from "styled-components";
import { useTranslation } from "react-i18next";
import { inject, observer } from "mobx-react";

import Text from "@docspace/components/text";
import SocialButton from "@docspace/components/social-button";

import { hugeMobile } from "@docspace/components/utils/device";
import { getAuthProviders } from "@docspace/common/api/settings";
import {
  getProviderTranslation,
  getOAuthToken,
  getLoginLink,
} from "@docspace/common/utils";
import { providersData } from "@docspace/common/constants";

const StyledWrapper = styled.div`
  display: flex;
  flex-direction: column;
  gap: 12px;

  .buttons {
    display: grid;
    grid-template-columns: 1fr 1fr;
    grid-column-gap: 20px;
    grid-row-gap: 12px;

    @media ${hugeMobile} {
      grid-template-columns: 1fr;
    }
  }
`;

const SocialNetworks = (props) => {
  const { t } = useTranslation(["Profile", "Common"]);
  const { providers, setProviders } = props;

  const fetchData = async () => {
    try {
      const data = await getAuthProviders();
      setProviders(data);
    } catch (e) {
      console.error(e);
    }
  };

  const linkAccount = async (providerName, link, e) => {
    e.preventDefault();

    try {
      const tokenGetterWin = window.open(
        link,
        "login",
        "width=800,height=500,status=no,toolbar=no,menubar=no,resizable=yes,scrollbars=no"
      );

      const code = await getOAuthToken(tokenGetterWin);
      const token = window.btoa(
        JSON.stringify({
          auth: providerName,
          mode: "popup",
          callback: "loginCallback",
        })
      );
      tokenGetterWin.location.href = getLoginLink(token, code);
    } catch (err) {
      console.log(err);
    }
  };

  useEffect(() => {
    fetchData();
  }, []);

  const providerButtons =
    providers &&
    providers.map((item) => {
      if (!providersData[item.provider]) return;
      const { icon, label, iconOptions } = providersData[item.provider];
      if (!icon || !label) return <></>;

      return (
        <div key={`${item.provider}ProviderItem`}>
          <SocialButton
            iconName={icon}
            label={getProviderTranslation(label, t)}
            $iconOptions={iconOptions}
            onClick={(e) => linkAccount(item.provider, item.url, e)}
          />
        </div>
      );
    });

  return (
    <StyledWrapper>
      <Text fontSize="16px" fontWeight={700}>
        {t("ConnectSocialNetworks")}
      </Text>
      <div className="buttons">{providerButtons}</div>
    </StyledWrapper>
  );
};

export default inject(({ peopleStore }) => {
  const { usersStore } = peopleStore;
  const { providers, setProviders } = usersStore;

  return {
    providers,
    setProviders,
  };
})(observer(SocialNetworks));
