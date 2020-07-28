import React, { useState } from "react";
import { Box, Text, Icons, toastr } from "asc-web-components";
import RecoverAccessModalDialog from "./recover-access-modal-dialog";
import styled from "styled-components";
import PropTypes from 'prop-types';
import { sendRecoverRequest } from "../../../../src/api/settings/index";

const RecoverContainer = styled(Box)`
  cursor: pointer;

  .recover-icon {
   @media(max-width: 450px) {
    padding: 16px;
   }
}
  .recover-text {
   @media(max-width: 450px) {
    display: none;
   }
}
`;

const RecoverAccess = ({ t }) => {

    const [visible, setVisible] = useState(false);
    const [loading, setLoading] = useState(false);

    const [email, setEmail] = useState("");
    const [emailErr, setEmailErr] = useState(false);

    const [description, setDescription] = useState("");
    const [descErr, setDescErr] = useState(false);

    const onRecoverClick = () => {
        setVisible(true);
    };
    const onRecoverModalClose = () => {
        setVisible(false);
        setEmail("");
        setEmailErr(false);
        setDescription("");
        setDescErr(false);
    };
    const onChangeEmail = (e) => {
        setEmail(e.currentTarget.value);
        setEmailErr(false);
    };
    const onChangeDescription = (e) => {
        setDescription(e.currentTarget.value);
        setDescErr(false);
    };
    const onSendRecoverRequest = () => {
        if (!email.trim()) {
            setEmailErr(true);
        }
        if (!description.trim()) {
            setDescErr(true);
        }
        else {
            setLoading(true);
            sendRecoverRequest(email, description)
                .then(
                    res => {
                        setLoading(false);
                        toastr.success(res);
                    },
                    message => {
                        setLoading(false);
                        toastr.error(message);
                    }
                )
                .finally(onRecoverModalClose)
        }
    };

    return (
        <>
            <Box
                widthProp="100%"
                heightProp="100%"
                displayProp="flex"
                justifyContent="flex-end"
                alignItems="center">
                <RecoverContainer
                    backgroundProp="#27537F"
                    heightProp="100%"
                    displayProp="flex"
                    onClick={onRecoverClick}>
                    <Box paddingProp="16px 8px 16px 16px" className="recover-icon">
                        <Icons.UnionIcon />
                    </Box>
                    <Box paddingProp="18px 16px 18px 0px" className="recover-text" widthProp="100%">
                        <Text color="#fff" isBold={true}>
                            {t("RecoverAccess")}
                        </Text>
                    </Box>
                </RecoverContainer>
            </Box>
            {visible && <RecoverAccessModalDialog
                visible={visible}
                loading={loading}
                email={email}
                emailErr={emailErr}
                description={description}
                descErr={descErr}
                t={t}
                onChangeEmail={onChangeEmail}
                onChangeDescription={onChangeDescription}
                onRecoverModalClose={onRecoverModalClose}
                onSendRecoverRequest={onSendRecoverRequest}
            />
            }
        </>
    )
}

RecoverAccess.propTypes = {
    t: PropTypes.func.isRequired
};

export default RecoverAccess;
