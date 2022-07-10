import os
import docker
import time
from dotenv import load_dotenv

load_dotenv()
client = docker.from_env()

api_dockerfile = os.environ.get("TECH_AT_HUB_PATH") + "/Tech@HubAPI"
api_image_name = "techhubapi-integration-test"
api_container_name = api_image_name
git_server_dockerfile = os.environ.get("TECH_AT_HUB_PATH") + "/Tech@HubGitServer"
git_server_image_name = "techhubgitserver-integration-test"
git_server_container_name = git_server_image_name
mysql_container_name = "mysql-techhub-integration-test"
network_name = "techathub"

api_container = None
git_server_container = None
mysql_container = None
network = None


def start_containers():
    global api_container
    global git_server_container
    global mysql_container
    global network

    print("Building Docker Images...")
    (api_image, logs) = client.images.build(
        path=api_dockerfile,
        tag=api_image_name,
        pull=True,
        dockerfile="Tech@HubAPI/Dockerfile",
    )

    (git_server_image, logs) = client.images.build(
        path=git_server_dockerfile,
        tag=git_server_image_name,
        pull=True,
        dockerfile="Dockerfile",
    )

    mysql_image = client.images.pull(repository="mysql", tag="latest")

    print("Creating Network...")
    network = client.networks.create(network_name, driver="bridge")

    print("Starting Containers...")
    git_dir = os.environ.get("TECH_AT_HUB_PATH") + "/git"

    mysql_container = client.containers.run(
        image="mysql",
        detach=True,
        name=mysql_container_name,
        ports={"3306/tcp": 3306},
        network=network_name,
        hostname="mysql",
        environment={
            "MYSQL_ROOT_PASSWORD": "dbpass",
            "MYSQL_DATABASE": "tech-at-hub",
            "MYSQL_USER": "gituser",
            "MYSQL_PASSWORD": "dbpass",
        },
    )

    time.sleep(25)

    api_container = client.containers.run(
        image=api_image_name,
        detach=True,
        name=api_container_name,
        ports={"80/tcp": 5000},
        volumes=[f"{git_dir}/:/app/git"],
        network=network_name,
        environment={
            "ASPNETCORE_URLS": "http://+:80",
            "ConnectionStrings__MySqlDatabase": "server=mysql; port=3306; uid=gituser; password=dbpass; database=tech-at-hub;",
        },
    )

    git_server_container = client.containers.run(
        image=git_server_image_name,
        detach=True,
        name=git_server_container_name,
        ports={"80/tcp": 80},
        volumes=[f"{git_dir}/:/app/git"],
        network=network_name,
    )

    time.sleep(7)


def stop_containers():
    print("Stopping Containers...")
    api_container.kill()
    api_container.remove()

    git_server_container.kill()
    git_server_container.remove()

    mysql_container.kill()
    mysql_container.remove()

    network.remove()
