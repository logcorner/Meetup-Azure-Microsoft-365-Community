# provider "kubernetes" {

# }



# resource "kubernetes_service" "example" {
#   metadata {
#     name = "terraform-example"
#     annotations = {
#       "service.beta.kubernetes.io/azure-load-balancer-internal" = "true"
#     }
#   }
#   spec {
#     selector = {
#       app = kubernetes_pod.example.metadata.0.labels.app
#     }
#     session_affinity = "ClientIP"
#     port {
#       port        = 80
#       target_port = 80
#     }

#     type = "LoadBalancer"
#   }
# }

# resource "kubernetes_pod" "example" {
#   metadata {
#     name = "terraform-example"
#     labels = {
#       app = "MyApp"
#     }
#   }

#   spec {
#     container {
#       image = "nginx:1.7.9"
#       name  = "example"
#     }
#   }
# }