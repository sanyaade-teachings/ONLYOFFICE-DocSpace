import React, { useEffect, useState } from "react";
import styled from "styled-components";
import { withRouter } from "react-router";
import { useTranslation, Trans } from "react-i18next";
import PropTypes from "prop-types";
import Loaders from "@docspace/common/components/Loaders";
import { setDocumentTitle } from "@docspace/client/src/helpers/filesUtils";
import { inject, observer } from "mobx-react";
import Text from "@docspace/components/text";
import CurrentTariffContainer from "./CurrentTariffContainer";
import PriceCalculation from "./PriceCalculation";
import BenefitsContainer from "./BenefitsContainer";
import { smallTablet } from "@docspace/components/utils/device";
import ContactContainer from "./ContactContainer";
import toastr from "@docspace/components/toast/toastr";
import moment from "moment";
import { HelpButton } from "@docspace/components";
import PayerInformationContainer from "./PayerInformationContainer";
import { TariffState } from "@docspace/common/constants";
import { getUserByEmail } from "@docspace/common/api/people";

const StyledBody = styled.div`
  max-width: 660px;

  .payment-info_suggestion,
  .payment-info_grace-period {
    margin-bottom: 12px;
  }

  .payment-info {
    display: grid;
    grid-template-columns: repeat(2, minmax(100px, 320px));
    grid-gap: 20px;
    margin-bottom: 20px;
    @media ${smallTablet} {
      grid-template-columns: 1fr;
      grid-template-rows: 1fr 1fr;
    }
  }
  .payment-info_wrapper {
    display: flex;
    margin-bottom: 18px;
    margin-top: 11px;
    div {
      margin: auto 0;
    }
    .payment-info_managers-price {
      margin-right: 6px;
    }
  }
`;

let paymentTerm,
  isValidDelayDueDate,
  fromDate,
  byDate,
  delayDaysCount,
  payerInfo = null;
const PaymentsPage = ({
  setPortalPaymentQuotas,
  language,
  isFreeTariff,
  isGracePeriod,
  theme,
  setPaymentAccount,
  isNotPaidPeriod,
  getSettingsPayment,
  setRangeBound,
  payerEmail,
  user,
  isPaidPeriod,
  currencySymbol,
  startValue,
  dueDate,
  delayDueDate,
  portalStatus,
  replaceFeaturesValues,
  portalPaymentQuotasFeatures,
  currentTariffPlanTitle,
  tariffPlanTitle,
}) => {
  const { t, ready } = useTranslation(["Payments", "Settings"]);

  const [isInitialLoading, setIsInitialLoading] = useState(true);

  const isAlreadyPaid = payerEmail.length !== 0 || !isFreeTariff;

  useEffect(() => {
    setDocumentTitle(t("Settings:Payments"));
  }, [ready]);
  useEffect(() => {
    if (ready && portalPaymentQuotasFeatures.length !== 0)
      replaceFeaturesValues(t);
  }, [ready, portalPaymentQuotasFeatures]);

  const gracePeriodDays = () => {
    const fromDateMoment = moment(dueDate);
    const byDateMoment = moment(delayDueDate);

    fromDate = fromDateMoment.format("L");
    byDate = byDateMoment.format("L");

    delayDaysCount = fromDateMoment.to(byDateMoment, true);
  };

  const setPortalDates = () => {
    paymentTerm = moment(
      isGracePeriod || isNotPaidPeriod ? delayDueDate : dueDate
    ).format("LL");
    const paymentTermYear = moment(delayDueDate).year();

    isValidDelayDueDate = paymentTermYear !== 9999;

    isGracePeriod && gracePeriodDays();
  };

  useEffect(() => {
    (async () => {
      moment.locale(language);

      const requests = [];

      requests.push(getSettingsPayment());

      if (!currencySymbol && !startValue)
        requests.push(setPortalPaymentQuotas());

      if (portalStatus !== TariffState.Trial)
        requests.push(setPaymentAccount());

      try {
        await Promise.all(requests);
        setRangeBound();
        setPortalDates();
      } catch (error) {
        toastr.error(error);
      }

      try {
        if (isAlreadyPaid) payerInfo = await getUserByEmail(payerEmail);
      } catch (e) {
        console.error(e);
      }

      setIsInitialLoading(false);
    })();
  }, []);

  const renderTooltip = () => {
    return (
      <>
        <HelpButton
          iconName={"/static/images/help.react.svg"}
          tooltipContent={
            <>
              <Text isBold>{t("ManagerTypesDescription")}</Text>
              <br />
              <Text isBold>{t("Administrator")}</Text>
              <Text>{t("AdministratorDescription")}</Text>
              <br />
              <Text isBold>{t("RoomManager")}</Text>
              <Text>{t("RoomManagerDescription")}</Text>
            </>
          }
        />
      </>
    );
  };

  const currentPlanTitle = () => {
    if (isPaidPeriod || isGracePeriod) {
      return (
        <Text noSelect fontSize="16px" isBold>
          <Trans t={t} i18nKey="BusinessTitle" ns="Payments">
            {{ planName: currentTariffPlanTitle }}
          </Trans>
        </Text>
      );
    }

    return (
      <Text noSelect fontSize="16px" isBold>
        <Trans t={t} i18nKey="StartupTitle" ns="Payments">
          {{ planName: currentTariffPlanTitle }}
        </Trans>
      </Text>
    );
  };

  const expiredTitleSubscriptionWarning = () => {
    return (
      <Text
        noSelect
        fontSize="16px"
        isBold
        color={theme.client.settings.payment.warningColor}
      >
        <Trans t={t} i18nKey="BusinessExpired" ns="Payments">
          {{ date: paymentTerm }} {{ planName: tariffPlanTitle }}
        </Trans>
      </Text>
    );
  };

  const planSuggestion = () => {
    if (isFreeTariff) {
      return (
        <Text
          noSelect
          fontSize="16px"
          isBold
          className={"payment-info_suggestion"}
        >
          <Trans t={t} i18nKey="StartupSuggestion" ns="Payments">
            {{ planName: tariffPlanTitle }}
          </Trans>
        </Text>
      );
    }

    if (isPaidPeriod) {
      return (
        <Text
          noSelect
          fontSize="16px"
          isBold
          className={"payment-info_suggestion"}
        >
          <Trans t={t} i18nKey="BusinessSuggestion" ns="Payments">
            {{ planName: tariffPlanTitle }}
          </Trans>
        </Text>
      );
    }

    if (isNotPaidPeriod) {
      return (
        <Text
          noSelect
          fontSize="16px"
          isBold
          className={"payment-info_suggestion"}
        >
          <Trans t={t} i18nKey="RenewSubscription" ns="Payments">
            {{ planName: tariffPlanTitle }}
          </Trans>
        </Text>
      );
    }

    if (isGracePeriod) {
      return (
        <Text
          noSelect
          fontSize="16px"
          isBold
          className={"payment-info_grace-period"}
          color={theme.client.settings.payment.warningColor}
        >
          <Trans t={t} i18nKey="DelayedPayment" ns="Payments">
            {{ date: paymentTerm }} {{ planName: currentTariffPlanTitle }}
          </Trans>
        </Text>
      );
    }

    return;
  };

  const isPayer = user.email === payerEmail;

  const isFreeAfterPaidPeriod = isFreeTariff && payerEmail.length !== 0;

  return isInitialLoading || !ready ? (
    <Loaders.PaymentsLoader />
  ) : (
    <StyledBody theme={theme}>
      {isNotPaidPeriod && isValidDelayDueDate
        ? expiredTitleSubscriptionWarning()
        : currentPlanTitle()}

      {isAlreadyPaid && (
        <PayerInformationContainer
          payerInfo={payerInfo}
          isPayer={isPayer}
          payerEmail={payerEmail}
        />
      )}

      <CurrentTariffContainer />

      {planSuggestion()}

      {isGracePeriod && (
        <Text noSelect fontSize={"14px"} lineHeight={"16px"}>
          <Trans t={t} i18nKey="GracePeriodActivatedDescription" ns="Payments">
            Grace period activated from <strong>{{ fromDate }}</strong> -
            <strong>{{ byDate }}</strong> (<strong>{{ delayDaysCount }}</strong>
            ).
          </Trans>
        </Text>
      )}

      {isPaidPeriod && !isFreeTariff && (
        <Text
          noSelect
          fontSize={"14px"}
          lineHeight={"16px"}
          className="payment-info_managers-price"
        >
          <Trans t={t} i18nKey="BusinessFinalDateInfo" ns="Payments">
            {{ finalDate: paymentTerm }}
          </Trans>
        </Text>
      )}

      <div className="payment-info_wrapper">
        <Text
          noSelect
          fontWeight={600}
          fontSize={"14px"}
          className="payment-info_managers-price"
        >
          <Trans t={t} i18nKey="PerUserMonth" ns="Payments">
            From {{ currencySymbol }}
            {{ price: startValue }} per admin/month
          </Trans>
        </Text>

        {renderTooltip()}
      </div>
      <div className="payment-info">
        <PriceCalculation
          t={t}
          isPayer={isPayer}
          isAlreadyPaid={isAlreadyPaid}
          isFreeAfterPaidPeriod={isFreeAfterPaidPeriod}
        />

        {isGracePeriod || isNotPaidPeriod || isFreeAfterPaidPeriod ? (
          <></>
        ) : (
          <BenefitsContainer t={t} />
        )}
      </div>
      <ContactContainer t={t} />
    </StyledBody>
  );
};

PaymentsPage.propTypes = {
  isLoaded: PropTypes.bool,
};

export default inject(({ auth, payments }) => {
  const {
    language,
    currentQuotaStore,
    paymentQuotasStore,
    currentTariffStatusStore,
    userStore,
  } = auth;

  const { isFreeTariff, currentTariffPlanTitle } = currentQuotaStore;
  const {
    isNotPaidPeriod,
    isPaidPeriod,
    isGracePeriod,
    customerId,
    dueDate,
    delayDueDate,
    portalStatus,
  } = currentTariffStatusStore;

  const {
    setPortalPaymentQuotas,
    planCost,
    replaceFeaturesValues,
    portalPaymentQuotasFeatures,
    tariffPlanTitle,
  } = paymentQuotasStore;
  const { organizationName, theme } = auth.settingsStore;

  const {
    tariffsInfo,
    setPaymentAccount,
    getSettingsPayment,
    setRangeBound,
  } = payments;

  const { user } = userStore;

  return {
    isFreeTariff,
    tariffPlanTitle,
    language,
    organizationName,
    tariffsInfo,
    isGracePeriod,
    theme,
    setPaymentAccount,
    currencySymbol: planCost.currencySymbol,
    startValue: planCost.value,
    isNotPaidPeriod,
    getSettingsPayment,
    setRangeBound,
    payerEmail: customerId,
    user,
    isPaidPeriod,
    setPortalPaymentQuotas,
    dueDate,
    delayDueDate,
    portalStatus,
    replaceFeaturesValues,
    portalPaymentQuotasFeatures,
    currentTariffPlanTitle,
  };
})(withRouter(observer(PaymentsPage)));
