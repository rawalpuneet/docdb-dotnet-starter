DOCDB-USER ?= docdb
DOCDB-PASS ?= docdb123
REGION ?= us-east-1
APP ?= docdb-dotnet-starter
LINUX2-AMI=ami-02538f8925e3aa27a
KEYPAIR-NAME=ec2-keypair
DOCDB-CLUSTER=$(shell aws cloudformation describe-stacks --region $(REGION) --stack-name $(APP) --query "Stacks[0].Outputs[?OutputKey=='DocumentDBCluster'].OutputValue | [0]" --output text)
EC2-PUBLICDNS=$(shell aws cloudformation describe-stacks --region $(REGION) --stack-name $(APP) --query "Stacks[0].Outputs[?OutputKey=='Ec2PublicDns'].OutputValue | [0]" --output text)




infra:
	@echo "\n\n########################################"
	@echo "     ==> Starting cloudformation deployment ! <==\n"
	@echo "########################################\n" 
	aws cloudformation update-stack --stack-name $(APP) --template-body file://cfn/template.yaml \
			--parameters ParameterKey=DocDBUsername,ParameterValue=$(DOCDB-USER) \
						 ParameterKey=DocDBPassword,ParameterValue=$(DOCDB-PASS) \
						 ParameterKey=Ec2Ami,ParameterValue=$(LINUX2-AMI) \
						 ParameterKey=Ec2KeyPair,ParameterValue=$(KEYPAIR-NAME) \
			--capabilities CAPABILITY_NAMED_IAM
tunnel:
	@echo "Setting up tunnel for remote access \n"
	ssh -i "$(KEYPAIR-NAME).pem" -L 27017:$(DOCDB-CLUSTER):27017 ec2-user@$(EC2-PUBLICDNS) -N


ec2:
	@echo "connecting to Ec2 machine setup by stack \n"
	ssh -i "$(KEYPAIR-NAME).pem" ec2-user@$(EC2-PUBLICDNS)