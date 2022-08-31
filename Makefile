# Update Here : DocumentDB Username
DOCDB-USER ?= docdb
# Update Here : DocumentDB Password
DOCDB-PASS ?= docdb123
# Update Here : Update the name of the key value pair for EC2 host needed to setup ssh tunnel to DocumentDB
KEYPAIR-NAME=ec2-keypair
# Update Here : Default region us-east-1. Change here if you need any other. 
REGION ?= us-east-1
# Update Here : APP is used as a cloudformation stack name. 
APP ?= docdb-dotnet-starter
# Amazon Linux 2 AMI ID. Change only if you need to use any other linux amis
LINUX2-AMI=ami-02538f8925e3aa27a

# Gets the cluster endpoint from cloudformaiton stack. 
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
	ssh -i "keys/$(KEYPAIR-NAME).pem" -L 27017:$(DOCDB-CLUSTER):27017 ec2-user@$(EC2-PUBLICDNS) -N


ec2:
	@echo "connecting to Ec2 machine setup by stack \n"
	ssh -i "keys/$(KEYPAIR-NAME).pem" ec2-user@$(EC2-PUBLICDNS)